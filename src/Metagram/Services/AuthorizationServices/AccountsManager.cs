using Metagram.Models.Authorization;
using Metagram.Models.Polling;
using Metagram.Services.AuthorizationServices.Abstractions;
using System.Collections.ObjectModel;

namespace Metagram.Services.AuthorizationServices;

public sealed class AccountsManager : IAccountsManager
{
    private readonly ILogger<AccountsManager> _logger;

    public ObservableCollection<BotRuntimeSession> Sessions { get; } = [];

    public AccountsManager(ILogger<AccountsManager> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (BotRuntimeSession session in Sessions)
            await StartSession(session, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (BotRuntimeSession session in Sessions)
            await session.StopAsync(cancellationToken);
    }

    public void Login(BotAccountInfo account)
    {
        try
        {
            BotMemory botMemory = new BotMemory();
            BotRuntimeSession session = new BotRuntimeSession(account, botMemory);
            Application.Current.Dispatcher.Invoke(() => Sessions.Add(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to login");
            throw;
        }
    }

    public async Task StartSession(BotRuntimeSession session, CancellationToken cancellationToken = default)
    {
        try
        {
            await session.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start bot's account session execution");
            throw;
        }
    }
}
