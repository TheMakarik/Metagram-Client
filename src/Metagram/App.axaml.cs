using System.IO;
using Metagram.Services.ApplicationData;

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


        LoadApplication();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ViewLocator locator = new ViewLocator();
        Locator.CurrentMutable.RegisterConstant<IViewLocator>(locator);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindowViewModel vm = Services.GetRequiredService<MainWindowViewModel>();
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
            .AddScoped<IApplicationDataDirectoryCreator, ApplicationDataDirectoryCreator>()
            .AddSingleton<IUpdateHandler, MetaUpdateHandler>()
            .AddSingleton<IAccountsManager, AccountsManager>()
            .AddDbContextFactory<MetagramDbContext>((provider, options) => options
                .EnableSensitiveDataLogging(false)
                .UseSqlite($"Data Source={provider.GetRequiredService<IOptions<ApplicationDataOptions>>().Value.Database};Pooling=True;Cache=Shared")
                .LogTo(message => provider.GetRequiredService<ILogger<App>>().LogDebug("SQLite Query : {message}", message))
            );

        services
            .AddSingleton<MainWindow>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<IScreen>(sp => sp.GetRequiredService<MainWindowViewModel>())
            .AddSingleton<LoginViewModel>()
            .AddSingleton<MessangerViewModel>();

        services
            .AddLogging(logging => logging
                .ClearProviders()
                .SetMinimumLevel(
                    #if DEBUG
                    LogLevel.Trace
                    #else
                    LogLevel.Information
                    #endif
                    )
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
    
    
    private void LoadApplication()
    {
    
        using IServiceScope scope = services.CreateScope();
     
        scope
            .ServiceProvider
            .GetRequiredService<IApplicationDataDirectoryCreator>()
            .CreateIfNotExists();
        
        //TODO: use migrations
        scope
            .ServiceProvider
            .GetRequiredService<MetagramDbContext>()
            .Database.EnsureCreated();
    }

}