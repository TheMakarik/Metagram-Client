using Metagram.Models.Authorization;

namespace Metagram.Controls;

public partial class AccountSelectorButton : UserControl
{
    public BotRuntimeSession SelectedSession
    {
        get => (BotRuntimeSession)GetValue(SelectedSessionProperty);
        set => SetValue(SelectedSessionProperty, value);
    }

    public event EventHandler<SessionSelectedEventArgs> SessionSelected
    {
        add => AddHandler(SessionSelectedEvent, value);
        remove => RemoveHandler(SessionSelectedEvent, value);
    }

    public AccountSelectorButton()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new SessionSelectedEventArgs(SessionSelectedEvent, SelectedSession));
    }

    public static readonly DependencyProperty SelectedSessionProperty = DependencyProperty.Register(
        nameof(SelectedSession), typeof(BotRuntimeSession), typeof(AccountSelectorButton),
        new PropertyMetadata(defaultValue: null));

    public static readonly RoutedEvent SessionSelectedEvent = EventManager.RegisterRoutedEvent(
        nameof(SessionSelected), RoutingStrategy.Bubble,
        typeof(EventHandler<SessionSelectedEventArgs>), typeof(AccountSelectorButton));
}

public class SessionSelectedEventArgs(RoutedEvent routedEvent, BotRuntimeSession session) : RoutedEventArgs(routedEvent)
{
    public BotRuntimeSession Session { get; } = session;
}