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
        InitializeComponent();
        BotMemory = App.Services.GetService<IBotMemory>();
        AddHandler(ChatSelectorButton.ChatSelectedEvent, new EventHandler<ChatSelectedEventArgs>(OnChatSelected));
    }

    private void OnChatSelected(object? sender, ChatSelectedEventArgs e)
    {
        SelectedChat = e.Chat;
        //RaiseEvent(new ChatSelectedEventArgs(ChatChosenEvent, e.Chat));
    }

    private void OnMemoryChatsUpdated()
    {
        if (BotMemory == null)
            return;

        SelectedChat ??= BotMemory.Chats.FirstOrDefault();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.Property.Name)
        {
            case nameof(BotMemory):
                {
                    if (BotMemory == null)
                        break;

                    IBotMemory oldMemory = (IBotMemory)e.OldValue;
                    oldMemory.Chats.CollectionChanged -= (s, e) => OnMemoryChatsUpdated();

                    IBotMemory newMemory = (IBotMemory)e.NewValue;
                    newMemory.Chats.CollectionChanged += (s, e) => OnMemoryChatsUpdated();
                    break;
                }
        }
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