using Microsoft.Data.Sqlite;
using Metagram.Model.Options;
using Metagram.Services.AppDataServices;
using Microsoft.Extensions.Options;


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

        builder.Services
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>(s =>
                {
                    SqliteOptions options = s.GetRequiredService<IOptions<SqliteOptions>>().Value;
                    return new DatabaseInitializer(
                        new SqliteConnection(options.ConnectionString),
                        options.MessageTypes,
                        s.GetRequiredService<ILogger<DatabaseInitializer>>()
                    );
                }
                  
            ).AddTransient<MainWindow>(); //TO DO: Add ViewModelLocator 
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(Log.Logger, dispose: true);

        _app = builder.Build();
        _app.RunAsync(); //Dont use Run()
        
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        _app.Services.GetRequiredService<MainWindow>().Show();
        await _app.Services.GetRequiredService<IDatabaseInitializer>().InitializeAsync();
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