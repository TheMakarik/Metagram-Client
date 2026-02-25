namespace Metagram.Services.Authorization;

public interface IAccountsManager
{
    public ObservableCollection<BotRuntimeSession> Sessions { get; }

    public Task<BotRuntimeSession> CreateSession(BotAccountInfo account, CancellationToken cancellationToken = default);
    public Task RegisterSession(BotRuntimeSession session, CancellationToken cancellationToken = default);
}
