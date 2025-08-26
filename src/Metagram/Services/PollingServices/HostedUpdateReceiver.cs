namespace Metagram.Services.PollingServices;

public class HostedUpdateReceiver(
    ITelegramBotClient client,
    IUpdateHandler handler,
    ILogger<HostedUpdateReceiver> logger,
    IOptions<HostedUpdateReceiverOptions> options) : IHostedService, IUpdateReceiver
{
    private const string ReceiverWasStartedLogMessage = "Update receiver have been just started";
    private const string ReceiverWasStoppedLogMessage = "Update receiver was stopped";
    private const string HandlingErrorLogMessage = "Handling error with id {id} and exception {exception}";
    private const string HandlingUpdateLogMessage = "Handling update with id {id}";
    private const string HandlingOperationCancelledLogMessage = "Handling request operation was cancalled";
    private const string ExceptionOccurredLogMessage = "Exception {exception} occurred";
    
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private IUpdateHandler _updateHandler;
    
    
    public Task StartAsync(CancellationToken cancellationToken)
    { 
        logger.LogDebug(ReceiverWasStartedLogMessage);
        Task.Run(async () => await ReceiveAsync(handler, _cancellationToken.Token), _cancellationToken.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug(ReceiverWasStoppedLogMessage);
        _cancellationToken.Cancel();
        return Task.CompletedTask;
    }

    public async Task ReceiveAsync(IUpdateHandler updateHandler, CancellationToken cancellationToken = default)
    {
        cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken).Token;
        GetUpdatesRequest request = CreateUpdateRequestGetter();

        _updateHandler = updateHandler;
        while (!cancellationToken.IsCancellationRequested)
            await HandleUpdatesAsync(updateHandler, cancellationToken, request);
    }
    
    private async Task HandleUpdatesAsync(IUpdateHandler updateHandler, CancellationToken cancellationToken, GetUpdatesRequest request)
    {
        try
        {
            await HandleRequestUpdate(cancellationToken, request);
        }
        catch (OperationCanceledException)
        { 
            logger.LogWarning(HandlingOperationCancelledLogMessage);
        }
        catch (Exception exception)
        {
            logger.LogWarning(ExceptionOccurredLogMessage, exception.Message);
            await updateHandler.HandleErrorAsync(client, exception, HandleErrorSource.PollingError, cancellationToken);
        }
    }

    private async Task HandleRequestUpdate(CancellationToken cancellationToken, GetUpdatesRequest request)
    {
        foreach (Update update in await client.SendRequest(request, cancellationToken).ConfigureAwait(false))
        {
            request.Timeout = (int)client.Timeout.TotalSeconds;
            try
            {
                request.Offset = update.Id + 1;
                logger.LogInformation(HandlingUpdateLogMessage, update.Id);
                await _updateHandler.HandleUpdateAsync(client, update, cancellationToken);
            }
            catch (OperationCanceledException) { } 
            catch (Exception exception)
            {
                logger.LogInformation(HandlingErrorLogMessage, update.Id, exception);
                await _updateHandler.HandleErrorAsync(
                    client, 
                    exception,
                    HandleErrorSource.HandleUpdateError,
                    cancellationToken);
            }
        }
    }
    
    private GetUpdatesRequest CreateUpdateRequestGetter()
    {
        return new GetUpdatesRequest()
        {
            AllowedUpdates = options.Value.AllowedUpdates,
            Limit = options.Value.Limit,
            Offset = options.Value.Offset
        };
    }
}

