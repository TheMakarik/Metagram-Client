namespace Metagram.Services;

public interface IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken = default);
    public Task StopAsync(CancellationToken cancellationToken = default);
}
