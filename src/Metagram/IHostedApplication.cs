namespace Metagram;

internal interface IHostedApplication : IDisposable
{
     public IServiceProvider Services { get; }
     public IConfiguration Configuration { get; }
}

