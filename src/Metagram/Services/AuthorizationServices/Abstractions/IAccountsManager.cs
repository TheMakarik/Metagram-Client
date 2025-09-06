using Metagram.Models.Authorization;
using System.Collections.ObjectModel;

namespace Metagram.Services.AuthorizationServices.Abstractions;

public interface IAccountsManager : IHostedService
{
    public ObservableCollection<BotRuntimeSession> Sessions { get; }

    public void Login(BotAccountInfo account);
    public Task StartSession(BotRuntimeSession session, CancellationToken cancellationToken = default);
}
