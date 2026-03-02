namespace Metagram.Services.ApplicationData;

public sealed class ApplicationDataDirectoryCreator(ILogger<string> logger, IOptions<ApplicationDataOptions> applicationDataOptions) : IApplicationDataDirectoryCreator
{
    public void CreateIfNotExists()
    {
        var applicationData = applicationDataOptions.Value.ApplicationDataDirectory;
        if(Directory.Exists(applicationData))
            logger.LogInformation("Application data directory already exists at path {directory} and won't be created", applicationData);
        else 
            CreateApplicationDataDirectory(applicationData);
    }

    private void CreateApplicationDataDirectory(string applicationData)
    {
        logger.LogInformation("Creating application data directory {directory} at its content", applicationData);
        Directory.CreateDirectory(applicationData);
        Directory.CreateDirectory(applicationDataOptions.Value.Settings);
    }
}