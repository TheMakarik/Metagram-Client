namespace Metagram;

public sealed partial class App : IDisposable
{
    private const string ApplicationWasStoppedLogMessage = "Application was stopped with exit code {code}";
    private const string LogLevelSection = "Logging:LogLevel";
    private const string FileLoggingSection = "Logging:File";
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
        configuration
            .AddJsonFile(AppSettingsPath);

        services
            .ConfigureOptions(configuration)
            .AddScoped<ISqliteConnectionFactory, SqliteConnectionFactory>()
            .AddTransient<MainWindow>()
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTelegramBot()
            .AddPolling()
            .AddLogging(logging => logging
                .SetMinimumLevel(LogLevel.Trace) //I don't know why Enum.Parse from Logging:LogLevel do not work, need to put it into configuration
                .ClearProviders()
                .AddFile(configuration.GetSection(FileLoggingSection))
                .AddConsole()
            )
            .AddViewModelLocator(
                locator => locator
                    .AddViewModel<MainWindowViewModel, MainWindow>()
            );
    }

  

    protected override async void OnStartup(StartupEventArgs e)
    {
        foreach (IHostedService hostedService in Services.GetServices<IHostedService>())
            _hostedServices.Add(hostedService);

        // DO NOT REMOVE AWAITER
        // Services's StartAsync method should ONLY contain initializations
        // Every other background shit should be managed by service, not the host
        Task.WaitAll(_hostedServices.Select(srv => srv.StartAsync(default)).ToArray());

        using IServiceScope scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>().InitializeAsync();
        
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