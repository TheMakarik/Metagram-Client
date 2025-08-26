using Metagram.Services.AppDataServices;
using Microsoft.Data.Sqlite;

namespace Metagram;

public sealed partial class App : Application, IHostedApplication
{
    private const string ApplicationWasStoppedLogMessage = "Application was stopped with exit code {code}";
    private const string AppSettingsPath = "appsettings.json";
    
    private readonly ILogger<App> _logger;
    private readonly MainWindow _mainWindow;

    private bool _isDisposed;

    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }

    public App()
    {
        ServiceCollection servicesCollection = new ServiceCollection();
        ConfigurationManager configurationManager = new ConfigurationManager();
        servicesCollection.AddLogging(loggingBuilder => Configure(servicesCollection, configurationManager, loggingBuilder));

        Configuration = configurationManager;
        Services = servicesCollection.BuildServiceProvider();

        _logger = Services.GetRequiredService<ILogger<App>>();
        _mainWindow = Services.GetRequiredService<MainWindow>();
    }

    private void Configure(IServiceCollection services, IConfigurationManager configuration, ILoggingBuilder logging)
    {
        configuration
            .AddJsonFile(AppSettingsPath);

        //TO DO: Add ViewModelLocator 
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

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        logging
            .ClearProviders()
            .AddSerilog(Log.Logger, dispose: true)
            .AddConsole();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _mainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _logger.LogInformation(ApplicationWasStoppedLogMessage, e.ApplicationExitCode);
        base.OnExit(e);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        if (Services is IDisposable servicesDisposable)
            servicesDisposable?.Dispose();

        if (Configuration is IDisposable configurationDisposable)
            configurationDisposable?.Dispose();

        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}