using Metagram.Models.Polling;
using Metagram.Services.PollingServices.Abstractions;

namespace Metagram.Controls;

public partial class ChatsPanel
{
    public IBotMemory? BotMemory
    {
        get => (IBotMemory?)GetValue(BotMemoryProperty);
        set => SetValue(BotMemoryProperty, value);
    }

    public ChatMemory? SelectedChat
    {
        get => (ChatMemory?)GetValue(SelectedChatProperty);
        set => SetValue(SelectedChatProperty, value);
    }

    public event EventHandler<ChatSelectedEventArgs> ChatChosen
    {
        add => AddHandler(ChatChosenEvent, value);
        remove => RemoveHandler(ChatChosenEvent, value);
    }

    public ChatsPanel()
    {
        if (App.Services != null)
        {
            BotMemory = App.Services.GetRequiredService<IBotMemory>();
            SelectedChat = BotMemory.Chats.FirstOrDefault();
        }

        InitializeComponent();
        AddHandler(ChatSelectorButton.ChatSelectedEvent, new EventHandler<ChatSelectedEventArgs>(OnChatSelected));
    }

    private void OnChatSelected(object? sender, ChatSelectedEventArgs e)
    {
        SelectedChat = e.Chat;
        //RaiseEvent(new ChatSelectedEventArgs(ChatChosenEvent, e.Chat));
    }

    public static readonly DependencyProperty BotMemoryProperty = DependencyProperty.Register(
        nameof(BotMemory), typeof(IBotMemory), typeof(ChatsPanel),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedChatProperty = DependencyProperty.Register(
        nameof(SelectedChat), typeof(ChatMemory), typeof(ChatsPanel),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly RoutedEvent ChatChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(ChatChosen), RoutingStrategy.Bubble,
        typeof(EventHandler<ChatSelectedEventArgs>), typeof(ChatsPanel));
}