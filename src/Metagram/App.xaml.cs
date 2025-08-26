namespace Metagram;

public partial class App : Application, IHostedApplication
{
    private const string ApplicationWasStoppedLogMessage = "Application was stopped with exit code {code}";

    private readonly ILogger<App> _logger;
    private readonly MainWindow _mainWindow;

    private bool isDisposed = false;

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

    protected virtual void Configure(IServiceCollection services, IConfigurationManager configuration, ILoggingBuilder logging)
    {
        configuration
            .AddJsonFile("appsettings.json");

        //TO DO: Add ViewModelLocator 
        services
            .AddTransient<MainWindow>();

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
        if (isDisposed)
            return;

        if (Services is IDisposable servicesDisposable)
            servicesDisposable?.Dispose();

        if (Configuration is IDisposable configurationDisposable)
            configurationDisposable?.Dispose();

        GC.SuppressFinalize(this);
        isDisposed = true;
    }
}