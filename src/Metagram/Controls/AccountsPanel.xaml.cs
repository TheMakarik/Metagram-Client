using Metagram.Models.Authorization;
using Metagram.Services.AuthorizationServices.Abstractions;

namespace Metagram.Controls;

public partial class AccountsPanel : UserControl
{
    public IAccountsManager? AccountsManager
    {
        get => (IAccountsManager?)GetValue(AccountsManagerProperty);
        set => SetValue(AccountsManagerProperty, value);
    }

    public BotRuntimeSession? SelectedSession
    {
        get => (BotRuntimeSession?)GetValue(SelectedSessionProperty);
        set => SetValue(SelectedSessionProperty, value);
    }

    public AccountsPanel()
    {
        if (App.Services != null)
        {
            AccountsManager = App.Services.GetRequiredService<IAccountsManager>();
        }

        InitializeComponent();
        AddHandler(AccountSelectorButton.SessionSelectedEvent, new EventHandler<SessionSelectedEventArgs>(OnSessionSelected));
    }

    private void OnSessionSelected(object? sender, SessionSelectedEventArgs e)
    {
        SelectedSession = e.Session;
    }

    public static readonly DependencyProperty AccountsManagerProperty = DependencyProperty.Register(
        nameof(AccountsManager), typeof(IAccountsManager), typeof(AccountsPanel),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedSessionProperty = DependencyProperty.Register(
        nameof(SelectedSession), typeof(BotRuntimeSession), typeof(AccountsPanel),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
}
