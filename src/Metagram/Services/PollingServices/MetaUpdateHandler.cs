namespace Metagram.Services.PollingServices;

internal class MetaUpdateHandler(ILogger<MetaUpdateHandler> logger) : IUpdateHandler
{
    private const string ErrorHandlerLogMessage = "Exception occured during handling update. From {source}";
    private const string HandlerTracerLogMessage = "Received update ({id}, {type})";

    private readonly ILogger<MetaUpdateHandler> _logger = logger;

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, ErrorHandlerLogMessage, source);
        return Task.CompletedTask;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogTrace(HandlerTracerLogMessage, update.Id, update.Type);
        return Task.CompletedTask;
    }
}
