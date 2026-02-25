namespace Metagram.Models.Options;

public class ApplicationDataOptions
{
    public required string Database { get => CombineWithApplicationData(field); set; }
    public required string Settings { get =>  CombineWithApplicationData(field); set; }
    public required string ApplicationDataDirectory { get; set; }

    private string CombineWithApplicationData(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        
        string applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationDataDirectory);

        return path == ApplicationDataDirectory //If try to get ApplicationDataDirectory
            ? applicationDataDirectory :
            Path.Combine(applicationDataDirectory, path);
    }
}
