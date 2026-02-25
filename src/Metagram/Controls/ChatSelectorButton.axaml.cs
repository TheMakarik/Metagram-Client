using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace Metagram.Controls;

public partial class ChatSelectorButton : UserControl
{
    public ChatHistory? ChatHistory
    {
        get => GetValue(ChatHistoryProperty);
        set => SetValue(ChatHistoryProperty, value);
    }

    public event EventHandler<ChatSelectedEventArgs> ChatSelected
    {
        add => AddHandler(ChatSelectedEvent, value);
        remove => RemoveHandler(ChatSelectedEvent, value);
    }

    public ChatSelectorButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new ChatSelectedEventArgs(ChatSelectedEvent, ChatHistory));
    }

    public static readonly StyledProperty<ChatHistory?> ChatHistoryProperty =
        AvaloniaProperty.Register<ChatSelectorButton, ChatHistory?>(nameof(ChatHistory), defaultBindingMode: BindingMode.TwoWay);

    public static readonly RoutedEvent<ChatSelectedEventArgs> ChatSelectedEvent =
        RoutedEvent.Register<ChatSelectorButton, ChatSelectedEventArgs>(nameof(ChatSelected), RoutingStrategies.Bubble);
}

public class ChatSelectedEventArgs(RoutedEvent routedEvent, ChatHistory? chatMemory) : RoutedEventArgs(routedEvent)
{
    public ChatHistory? Chat { get; } = chatMemory;
}
