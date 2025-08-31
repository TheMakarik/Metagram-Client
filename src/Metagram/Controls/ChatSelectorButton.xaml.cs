using Metagram.Models.Polling;
using Telegram.Bot.Types;

namespace Metagram.Controls;

public partial class ChatSelectorButton
{
    public ChatMemory SelectedChat
    {
        get => (ChatMemory)GetValue(SelectedChatProperty);
        set => SetValue(SelectedChatProperty, value);
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

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new ChatSelectedEventArgs(ChatSelectedEvent, SelectedChat));
    }

    public static readonly DependencyProperty SelectedChatProperty = DependencyProperty.Register(
        nameof(SelectedChat), typeof(ChatMemory), typeof(ChatSelectorButton),
        new PropertyMetadata(defaultValue: null));

    public static readonly RoutedEvent ChatSelectedEvent = EventManager.RegisterRoutedEvent(
        nameof(ChatSelected), RoutingStrategy.Bubble,
        typeof(EventHandler<ChatSelectedEventArgs>), typeof(ChatSelectorButton));
}

public class ChatSelectedEventArgs(RoutedEvent routedEvent, ChatMemory chatMemory) : RoutedEventArgs(routedEvent)
{
    public ChatMemory Chat { get; } = chatMemory;
}