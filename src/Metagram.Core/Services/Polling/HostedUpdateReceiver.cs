namespace Metagram.Services.Polling;

// HANDS OFF, TheMakarik
public class HostedUpdateReceiver(
    ITelegramBotClient client,
    IUpdateHandler updateHandler,
    ILogger<HostedUpdateReceiver> logger,
    IOptions<HostedUpdateReceiverOptions> options) : IUpdateReceiver
{
    private const string ReceiverWasStartedLogMessage = "Update receiver have been just started";
    private const string ReceiverWasStoppedLogMessage = "Update receiver was stopped";
    private const string HandlingErrorLogMessage = "Handling error with id {id} and exception {exception}";
    private const string HandlingUpdateLogMessage = "Handling update with id '{id}' of type '{type}'";
    private const string ExceptionOccurredLogMessage = "Exception {exception} occurred";
    
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private readonly ITelegramBotClient _client = client;
    private readonly IUpdateHandler _updateHandler = updateHandler;
    private readonly HostedUpdateReceiverOptions _options = options.Value;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug(ReceiverWasStartedLogMessage);
        Task.Run(async () => await ReceiveAsync(null!, _cancellationToken.Token), _cancellationToken.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug(ReceiverWasStoppedLogMessage);
        _cancellationToken.Cancel();
        return Task.CompletedTask;
    }

    public async Task ReceiveAsync(IUpdateHandler _, CancellationToken cancellationToken = default)
    {
        GetUpdatesRequest request = CreateUpdateRequestGetter(_options);

        if (_options.DropPendingUpdates)
            await DropPendingUpdates(request, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            await HandleUpdatesAsync(_updateHandler, request, cancellationToken);
    }
    
    private async Task DropPendingUpdates(GetUpdatesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Update[] array = await _client.GetUpdates(-1, 1, 0, [], cancellationToken).ConfigureAwait(false);
            request.Offset = array.Length != 0 ? array[^1].Id + 1 : 0;
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private async Task HandleUpdatesAsync(IUpdateHandler updateHandler, GetUpdatesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            foreach (Update update in await _client.SendRequest(request, cancellationToken).ConfigureAwait(false))
                await HandleRequestUpdate(update, request, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _ = 0xBAD + 0xC0DE;
            return;
        }
        catch (Exception exception)
        {
            logger.LogError(ExceptionOccurredLogMessage, exception.Message);
            await updateHandler.HandleErrorAsync(_client, exception, HandleErrorSource.PollingError, cancellationToken);
        }
    }

    private async Task HandleRequestUpdate(Update update, GetUpdatesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            request.Timeout = (int)_client.Timeout.TotalSeconds;
            request.Offset = update.Id + 1;

            logger.LogTrace(HandlingUpdateLogMessage, update.Id, update.Type);
            await _updateHandler.HandleUpdateAsync(_client, update, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _ = 0xBAD + 0xC0DE;
            return;
        }
        catch (Exception exception)
        {
            logger.LogError(HandlingErrorLogMessage, update.Id, exception);
            await _updateHandler.HandleErrorAsync(_client, exception, HandleErrorSource.HandleUpdateError, cancellationToken);
        }
    }
    
    private static GetUpdatesRequest CreateUpdateRequestGetter(HostedUpdateReceiverOptions? options)
    {
        return new GetUpdatesRequest()
        {
            AllowedUpdates = options?.AllowedUpdates,
            Limit = options?.Limit ?? 100
        };
    }
}

