using Metagram.Services;
using Metagram.Services.AppDataServices;
using Metagram.Services.PollingServices;
using Metagram.Services.ViewServices;
using Metagram.ViewModels;
using Microsoft.Data.Sqlite;

namespace Metagram;

public sealed partial class App : Application, IDisposable
{
    private const string ApplicationWasStoppedLogMessage = "Application was stopped with exit code {code}";
    private const string AppSettingsPath = "appsettings.json";

    private readonly List<IHostedService> _hostedServices = [];
    private readonly ILogger<App> _logger;
    private readonly MainWindow _mainWindow;

    private bool _isDisposed;

    public static IServiceProvider Services { get; private set; } = default!;
    public static IConfiguration Configuration { get; private set; } = default!;

    public App()
    {
        // Creating and configuring app infrastructure
        ServiceCollection servicesCollection = new ServiceCollection();
        ConfigurationManager configurationManager = new ConfigurationManager();
        Configure(servicesCollection, configurationManager);

        // Building providers
        Configuration = configurationManager;
        Services = servicesCollection.BuildServiceProvider();

        // Resolving required services
        _logger = Services.GetRequiredService<ILogger<App>>();
        _mainWindow = Services.GetRequiredService<MainWindow>();
    }

    private static void Configure(IServiceCollection services, IConfigurationManager configuration)
    {
        // Configuration and settings
        configuration
            .AddJsonFile(AppSettingsPath);

        // TO DO: Add ViewModelLocator, DONE
        services
            .AddTransient<MainWindow>()
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>(s =>
            {
                SqliteOptions options = s.GetRequiredService<IOptions<SqliteOptions>>().Value;
                return new DatabaseInitializer(
                    new SqliteConnection(options.ConnectionString),
                    options.MessageTypes,
                    s.GetRequiredService<ILogger<DatabaseInitializer>>()
                );
            });

        // Hosted services
        services
            .AddHostedService<HostedUpdateReceiver>();

        // View models and locator
        services.AddViewModelLocator(locator => locator
            .AddViewModel<MainWindowViewModel, MainWindow>());

        /* Fuck it, i'll do it myself
        // Day 4 of waiting when'll TheMakarik remove SeriLog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        */

        services.AddLogging(logging => logging
            .ClearProviders()
            //.AddSerilog(Log.Logger, dispose: true)
            .AddConsole());

    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Resolving hosted services
        foreach (IHostedService hostedService in Services.GetServices<IHostedService>())
            _hostedServices.Add(hostedService);

        // DO NOT REMOVE AWAITER
        // Services's StartAsync method should ONLY contain initializations
        // Every other background shit should be managed by service, not the host
        Task.WaitAll(_hostedServices.Select(srv => srv.StartAsync(default)).ToArray());

        _mainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // I removed feature with limited time to stop all background operations
        // Which is dangerous, but lets be optimistic :)
        Task.WaitAll(_hostedServices.Select(srv => srv.StopAsync(default)).ToArray());
        _hostedServices.Clear();

        _logger.LogInformation(ApplicationWasStoppedLogMessage, e.ApplicationExitCode);
        base.OnExit(e);
        Dispose();
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        if (Services is IDisposable servicesDisposable)
            servicesDisposable.Dispose();

        if (Configuration is IDisposable configurationDisposable)
            configurationDisposable.Dispose();

        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}