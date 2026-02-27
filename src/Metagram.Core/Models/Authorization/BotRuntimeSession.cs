using Microsoft.Extensions.Logging.Abstractions;

namespace Metagram.Models.Authorization;

public partial class BotRuntimeSession : IDisposable
{
    private readonly CancellationTokenSource _cancell;
    private readonly BotAccountInfo _account;
    private readonly BotMemory _memory;
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _handler;
    private readonly IUpdateReceiver _receiver;

    private User? loggedUser = null;
    private UserProfilePhotos? profilePhotos = null;
    private bool disposed = false;

    public ITelegramBotClient Client => _botClient;

    public BotMemory Memory => _memory;
    
    public BotAccountInfo Account => _account;
    
    public bool IsLoggedIn => loggedUser != null;

    public User LoggedUser => loggedUser ?? throw new AccountNotLoggedException();
    
    public UserProfilePhotos ProfilePhotos => profilePhotos ?? throw new AccountNotLoggedException();

    public BotRuntimeSession(BotAccountInfo account, BotMemory botMemory, IOptions<HostedUpdateReceiverOptions> options)
    {
        _account = account;
        _memory = botMemory;
        _botClient = new TelegramBotClient(account.GetOptions());
        _cancell = new CancellationTokenSource();

        _handler = new MetaUpdateHandler(new NullLogger<MetaUpdateHandler>(), this, _memory);
        _receiver = new HostedUpdateReceiver(_botClient, _handler, new NullLogger<HostedUpdateReceiver>(), options);
    }

    public async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        if (IsLoggedIn)
            throw new LoginException("Bot already loged in");

        try
        {
            loggedUser = await _account.Login(Client, cancellationToken);
            profilePhotos = await Client.GetUserProfilePhotos(LoggedUser.Id, cancellationToken: cancellationToken);
        }
        catch (Exception ex) when (ex is not LoginException)
        {
            throw new LoginException("Login attempt faulted", ex);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = _cancell.Token.LinkWith(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        _ = _receiver.ReceiveAsync(_handler, cancellationToken);
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

public sealed class AccountNotLoggedException() : Exception();
