namespace Metagram.Services.Polling;

internal class HostedUpdateReceiver(ITelegramBotClient client, IUpdateHandler handler) : IHostedService, IUpdateReceiver
{
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async ()
            => await ReceiveAsync(handler, _cancellationToken.Token), _cancellationToken.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationToken.Cancel();
        return Task.CompletedTask;
    }

    public async Task ReceiveAsync(IUpdateHandler updateHandler, CancellationToken cancellationToken = default)
    {
        cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken).Token;
        GetUpdatesRequest request = new GetUpdatesRequest()
        {
            AllowedUpdates = null,
            Limit = 100,
            Offset = null
        };

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                request.Timeout = (int)client.Timeout.TotalSeconds;
                foreach (Update update in await client.SendRequest(request, cancellationToken).ConfigureAwait(false))
                {
                    try
                    {
                        request.Offset = update.Id + 1;
                        await updateHandler.HandleUpdateAsync(client, update, cancellationToken);
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception exception)
                    {
                        await updateHandler.HandleErrorAsync(client, exception, HandleErrorSource.HandleUpdateError,
                            cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception exception)
            {
                await updateHandler.HandleErrorAsync(client, exception, HandleErrorSource.PollingError,
                    cancellationToken);
            }
        }
    }
}

