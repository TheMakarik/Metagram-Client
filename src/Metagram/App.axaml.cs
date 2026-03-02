namespace Metagram;

public partial class App : Application, IDisposable
{
    private IConfiguration configuration = null!;
    private IServiceProvider services = null!;
    private bool isDisposed = false;

    public static IServiceProvider Services => (Current as App)!.services;
    public static IConfiguration Configuration => (Current as App)!.configuration;

    public override void Initialize()
    {
        // Creating and configuring app infrastructure
        ServiceCollection servicesCollection = new ServiceCollection();
        ConfigurationManager configurationManager = new ConfigurationManager();
        Configure(servicesCollection, configurationManager);

        // Building providers
        configuration = configurationManager;
        services = servicesCollection.BuildServiceProvider();
        
        //TODO: use migrations
        using IServiceScope scope = services.CreateScope();
        scope.ServiceProvider.GetRequiredService<MetagramDbContext>().Database.EnsureCreated();
        
        
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ViewLocator locator = new ViewLocator();
        Locator.CurrentMutable.RegisterConstant<IViewLocator>(locator);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindowViewModel vm = Services.GetRequiredServiceWithParams<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow(vm);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void Configure(IServiceCollection services, IConfigurationManager configuration)
    {
        configuration
            .AddJsonFile("appsettings.json");

        services
            .Configure<ApplicationDataOptions>(configuration.GetSection(nameof(ApplicationDataOptions)))
            .Configure<HostedUpdateReceiverOptions>(configuration.GetSection(nameof(HostedUpdateReceiverOptions)));

        services
            .AddSingleton<IUpdateHandler, MetaUpdateHandler>()
            .AddSingleton<IAccountsManager, AccountsManager>()
            .AddDbContextFactory<MetagramDbContext>((provider, options) => options
                .UseSqlite($"Data Source={provider.GetRequiredServiceWithParams<IOptions<ApplicationDataOptions>>().Value.Database};Pooling=True;Cache=Shared;Max Pool Size=5")
                .LogTo(message => provider.GetRequiredService<ILogger<App>>().LogDebug("SQLite Query : {msg}", message))
            );

        services
            .AddSingleton<MainWindow>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<IScreen>(sp => sp.GetRequiredServiceWithParams<MainWindowViewModel>())
            .AddSingleton<LoginViewModel>()
            .AddSingleton<MessangerViewModel>();

        services
            .AddLogging(logging => logging
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddFile(configuration.GetSection("Logging"))
                .AddConsole()
            );
    }

    public void Dispose()
    {
        if (isDisposed)
            return;

        if (Services is IDisposable servicesDisposable)
            servicesDisposable.Dispose();

        if (Configuration is IDisposable configurationDisposable)
            configurationDisposable.Dispose();

        GC.SuppressFinalize(this);
        isDisposed = true;
    }
}