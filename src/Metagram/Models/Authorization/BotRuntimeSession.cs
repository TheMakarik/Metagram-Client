using Metagram.Models.Polling;

namespace Metagram.Models.Authorization;

public class BotRuntimeSession : IDisposable
{
    private readonly CancellationTokenSource _cancell;
    private readonly BotAccountInfo _account;
    private readonly BotMemory _memory;
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _handler;
    private readonly IUpdateReceiver _receiver;

    private bool disposed = false;

    public ITelegramBotClient Client => _botClient;
    public BotMemory Memory => _memory;
    public BotAccountInfo Account => _account;

    public BotRuntimeSession(BotAccountInfo account, BotMemory botMemory)
    {
        _account = account;
        _memory = botMemory;
        _botClient = new TelegramBotClient(account.GetOptions());
        _handler = ActivatorUtilities.CreateInstance<MetaUpdateHandler>(App.Services, _memory);
        _receiver = ActivatorUtilities.CreateInstance<HostedUpdateReceiver>(App.Services, _botClient, _handler);
        _cancell = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        cancellationToken = _cancell.Token.LinkWith(cancellationToken);
        Task.Run(async () => await _receiver.ReceiveAsync(_handler, cancellationToken));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _cancell.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (disposed)
            return;

        GC.SuppressFinalize(this);
        disposed = true;
    }
}
