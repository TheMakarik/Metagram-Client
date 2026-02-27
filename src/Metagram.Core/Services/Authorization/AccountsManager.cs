namespace Metagram.Services.Authorization;

public sealed class AccountsManager : IAccountsManager
{
    private readonly ILogger<AccountsManager> _logger;
    private readonly IOptions<HostedUpdateReceiverOptions> _options;
    private readonly Dictionary<string, BotAccountInfo> _accountsMap = [];

    public ObservableCollection<BotRuntimeSession> Sessions { get; } = [];

    public AccountsManager(ILogger<AccountsManager> logger, IOptions<HostedUpdateReceiverOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    public Task<BotRuntimeSession> CreateSession(BotAccountInfo account, CancellationToken cancellationToken = default)
    {
        if (_accountsMap.TryGetValue(account.Token, out _))
            throw new AlreadyLoggedException("This bot is already loged in");

        BotMemory botMemory = new BotMemory();
        BotRuntimeSession session = new BotRuntimeSession(account, botMemory, _options);
        return Task.FromResult(session);
    }

    public Task RegisterSession(BotRuntimeSession session, CancellationToken cancellationToken)
    {
        if (_accountsMap.TryGetValue(session.Account.Token, out _))
            throw new AlreadyLoggedException("This bot is already logged in");

        _accountsMap.Add(session.Account.Token, session.Account);
        Sessions.Add(session);
        return Task.CompletedTask;
    }
}

public static class AccountsManagerExtensions
{
    public static async Task<BotRuntimeSession> Login(this IAccountsManager accountsManager, BotAccountInfo accountInfo, CancellationToken cancellationToken = default)
    {
        BotRuntimeSession session = await accountsManager.CreateSession(accountInfo, cancellationToken);
        await session.LoginAsync(cancellationToken);
        await session.StartAsync(cancellationToken);
        await accountsManager.RegisterSession(session, cancellationToken);
        return session;
    }
}

public class LoginException(string message, Exception? innerException = null) : Exception(message, innerException);
public class AlreadyLoggedException(string message, Exception? innerException = null) : LoginException(message, innerException);
