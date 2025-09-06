using Metagram.Models.Polling;
using System.Net.Http;
using Telegram.Bot.Exceptions;

namespace Metagram.Services.PollingServices;

internal class MetaUpdateHandler(ILogger<MetaUpdateHandler> logger, BotMemory botMemory) : IUpdateHandler // IUpdateRouter
{
    private const string ErrorHandlerLogMessage = "Exception occured during handling update. From {source}";
    private const string HandlerTracerLogMessage = "Received update ({id}, {type})";

    private readonly ILogger<MetaUpdateHandler> _logger = logger;
    private readonly BotMemory _botMemory = botMemory;

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        if (exception is RequestException { InnerException: HttpRequestException httpRequestException })
        {
            _logger.LogError(httpRequestException.Message);
            return Task.CompletedTask;
        }

        _logger.LogError(exception, ErrorHandlerLogMessage, source);
        return Task.CompletedTask;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogTrace(HandlerTracerLogMessage, update.Id, update.Type);
        _botMemory.AddUpdate(update);
        return Task.CompletedTask;
    }
}
