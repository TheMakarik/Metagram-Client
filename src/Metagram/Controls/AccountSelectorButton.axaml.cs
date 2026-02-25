using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace Metagram.Controls;

public partial class AccountSelectorButton : UserControl
{
    public BotRuntimeSession? Session
    {
        get => GetValue(SessionProperty);
        set => SetValue(SessionProperty, value);
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

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new SessionSelectedEventArgs(SessionSelectedEvent, Session));
    }

    public static readonly StyledProperty<BotRuntimeSession?> SessionProperty = 
        AvaloniaProperty.Register<AccountSelectorButton, BotRuntimeSession?>(nameof(Session), defaultBindingMode: BindingMode.OneWay);

    public static readonly RoutedEvent<SessionSelectedEventArgs> SessionSelectedEvent =
        RoutedEvent.Register<AccountSelectorButton, SessionSelectedEventArgs>(nameof(SessionSelected), RoutingStrategies.Bubble);
}

public class SessionSelectedEventArgs(RoutedEvent routedEvent, BotRuntimeSession? session) : RoutedEventArgs(routedEvent)
{
    public BotRuntimeSession? Session { get; } = session;
}
