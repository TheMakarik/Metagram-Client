namespace Metagram.ViewModels;

public partial class MainWindowViewModel : ScreenViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IAccountsManager _accountsManager;

    [Reactive]
    private ICommand _routeTest;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IAccountsManager accountsManager)
    {
        _logger = logger;
        _accountsManager = accountsManager;

        _routeTest = ReactiveCommand.Create(() =>
        {
            this.NavigateTo<MessangerViewModel>();
            if (accountsManager.Sessions.Count == 0)
                this.NavigateTo<LoginViewModel>();
        });
    }
}
