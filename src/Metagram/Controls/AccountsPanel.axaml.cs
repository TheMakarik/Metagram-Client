namespace Metagram.Controls;

public partial class AccountsPanel : UserControl
{
    public IAccountsManager? AccountsManager
    {
        get => GetValue(AccountsManagerProperty);
        set => SetValue(AccountsManagerProperty, value);
    }

    public BotRuntimeSession? SelectedSession
    {
        get => GetValue(SelectedSessionProperty);
        set => SetValue(SelectedSessionProperty, value);
    }

    public ICommand? LoginCommand
    {
        get => GetValue(LoginCommandProperty);
        set => SetValue(LoginCommandProperty, value);
    }

    public event EventHandler<SessionSelectedEventArgs> SessionSelected
    {
        add => AddHandler(SessionSelectedEvent, value);
        remove => RemoveHandler(SessionSelectedEvent, value);
    }

    public AccountsPanel()
    {
        InitializeComponent();
        AddHandler(AccountSelectorButton.SessionSelectedEvent, OnSessionSelected);
    }

    private void OnSessionSelected(object? sender, SessionSelectedEventArgs e)
    {
        SelectedSession = e.Session;
        RaiseEvent(new SessionSelectedEventArgs(SessionSelectedEvent, SelectedSession));
    }

    private void OnMemorySessionsUpdated(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (AccountsManager == null)
            return;

        SelectedSession ??= AccountsManager.Sessions.FirstOrDefault();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == AccountsManagerProperty)
        {
            IAccountsManager? oldValue = change.GetOldValue<IAccountsManager?>();
            oldValue?.Sessions?.CollectionChanged -= OnMemorySessionsUpdated;

            IAccountsManager? newValue = change.GetNewValue<IAccountsManager?>();
            oldValue?.Sessions?.CollectionChanged += OnMemorySessionsUpdated;
        }
    }

    public static readonly StyledProperty<IAccountsManager?> AccountsManagerProperty =
        AvaloniaProperty.Register<AccountsPanel, IAccountsManager?>(nameof(AccountsManager), defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<BotRuntimeSession?> SelectedSessionProperty =
        AvaloniaProperty.Register<AccountsPanel, BotRuntimeSession?>(nameof(SelectedSession), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ICommand?> LoginCommandProperty =
        AvaloniaProperty.Register<AccountsPanel, ICommand?>(nameof(LoginCommand), defaultBindingMode: BindingMode.OneWay);

    public static readonly RoutedEvent<SessionSelectedEventArgs> SessionSelectedEvent =
        RoutedEvent.Register<AccountsPanel, SessionSelectedEventArgs>(nameof(SessionSelected), RoutingStrategies.Bubble);
}