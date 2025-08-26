namespace Metagram;

public partial class App
{
    private const string AppSettingsPath = "appsettings.json";
    private const string ApplicationWasStoppedLogMessage = "Application was stopped with exit code {code}";
    
    private readonly IHost _app;
    
    public App()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Configuration
            .AddJsonFile(AppSettingsPath);

        builder.Services.AddTransient<MainWindow>(); //TO DO: Add ViewModelLocator 
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(Log.Logger, dispose: true);

        _app = builder.Build();
        _app.RunAsync(); //Dont use Run()
        
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _app.Services.GetRequiredService<MainWindow>().Show();
        
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        using IServiceScope scope = _app.Services.CreateScope();
        
        scope.ServiceProvider.GetRequiredService<ILogger<App>>().
            LogInformation(ApplicationWasStoppedLogMessage, e.ApplicationExitCode);
        
        base.OnExit(e);
    }
}