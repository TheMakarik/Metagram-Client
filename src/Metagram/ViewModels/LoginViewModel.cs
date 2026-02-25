using System.Linq;
using System.Windows.Input;

namespace Metagram.ViewModels;

public partial class LoginViewModel : RoutableViewModelBase
{
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IAccountsManager _accountsManager;

    [Reactive]
    private string? tokenInput = "8367964096:AAFkiOR2Dsn0XViIFHJrWPmmtwc4zTNw6xI";

    [Reactive]
    private ICommand loginCommand;

    [Reactive]
    private ICommand closeCommand;

    [Reactive]
    private bool isLoading;

    [Reactive]
    private bool isCloseButtonVisible;

    public LoginViewModel(IScreen screen, ILogger<LoginViewModel> logger, IAccountsManager accountsManager) : base("login", screen)
    {
        _logger = logger;
        _accountsManager = accountsManager;

        loginCommand = ReactiveCommand.Create(TryLogin);
        closeCommand = ReactiveCommand.Create(Close);

        IsCloseButtonVisible = _accountsManager.Sessions.Any();
    }

    private void Close()
    {
        HostScreen.NavigateBack();
        return;
    }

    private async Task TryLogin()
    {
        try
        {
            if (string.IsNullOrEmpty(TokenInput))
            {
                //await _navigator.ShowMessageDialogAsync(this, content: "Token cannot be null or empty");
                _logger.LogError("Token cannot be null or empty");
                return;
            }

            IsLoading = true;
            BotAccountInfo accountInfo = new BotAccountInfo(TokenInput);
            await _accountsManager.Login(accountInfo);

            IsCloseButtonVisible = true;
            HostScreen.NavigateBack();
        }
        catch (LoginException lex)
        {
            //await _navigator.ShowMessageDialogAsync(this, content: lex.Message);
            _logger.LogError(lex, null);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
