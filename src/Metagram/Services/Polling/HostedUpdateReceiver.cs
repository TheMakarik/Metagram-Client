using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Metagram.Services.Polling
{
    internal class HostedUpdateReceiver(ITelegramBotClient client, IUpdateHandler handler) : IHostedService, IUpdateReceiver
    {
        private readonly ITelegramBotClient _client = client;
        private readonly IUpdateHandler _handler = handler;
        private readonly CancellationTokenSource _cancell = new CancellationTokenSource();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => await ReceiveAsync(_handler, _cancell.Token), _cancell.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancell.Cancel();
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
                    request.Timeout = (int)_client.Timeout.TotalSeconds;
                    foreach (Update update in await _client.SendRequest(request, cancellationToken).ConfigureAwait(false))
                    {
                        try
                        {
                            request.Offset = update.Id + 1;
                            await updateHandler.HandleUpdateAsync(_client, update, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            continue;
                        }
                        catch (Exception exception)
                        {
                            await updateHandler.HandleErrorAsync(_client, exception, HandleErrorSource.HandleUpdateError, cancellationToken);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception exception)
                {
                    await updateHandler.HandleErrorAsync(_client, exception, HandleErrorSource.PollingError, cancellationToken);
                }
            }
        }
    }
}
