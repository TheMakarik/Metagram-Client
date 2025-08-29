namespace Metagram;

public sealed partial class App : IHostedApplication
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
            .Configure<HostedUpdateReceiverOptions>(configuration.GetSection(nameof(HostedUpdateReceiverOptions)))
            .Configure<SqliteOptions>(configuration.GetSection(nameof(SqliteOptions)));
        
        services
            .AddScoped<ISqliteConnectionFactory, SqliteConnectionFactory>()
            .AddTransient<MainWindow>()
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        logging
            .ClearProviders()
            .AddSerilog(Log.Logger, dispose: true);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        using IServiceScope scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>().InitializeAsync();
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

        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}