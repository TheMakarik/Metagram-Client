using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using System.Collections.Specialized;
using System.Linq;

namespace Metagram.Controls;

public partial class ChatsPanel : UserControl
{
    public BotMemory? BotMemory
    {
        get => GetValue(BotMemoryProperty);
        set => SetValue(BotMemoryProperty, value);
    }

    public ChatHistory? SelectedChat
    {
        get => GetValue(SelectedChatProperty);
        set => SetValue(SelectedChatProperty, value);
    }

    public event EventHandler<ChatSelectedEventArgs> ChatSelected
    {
        add => AddHandler(ChatSelectedEvent, value);
        remove => RemoveHandler(ChatSelectedEvent, value);
    }

    public ChatsPanel()
    {
        InitializeComponent();
        AddHandler(ChatSelectorButton.ChatSelectedEvent, OnChatSelected);
    }

    private void OnChatSelected(object? sender, ChatSelectedEventArgs e)
    {
        SelectedChat = e.Chat;
        RaiseEvent(new ChatSelectedEventArgs(ChatSelectedEvent, e.Chat));
    }

    private void OnMemoryChatsUpdated(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (BotMemory == null)
            return;

        SelectedChat ??= BotMemory.Chats.FirstOrDefault();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BotMemoryProperty)
        {
            BotMemory? oldValue = change.GetOldValue<BotMemory?>();
            oldValue?.Chats?.CollectionChanged -= OnMemoryChatsUpdated;

            BotMemory? newValue = change.GetNewValue<BotMemory?>();
            oldValue?.Chats?.CollectionChanged += OnMemoryChatsUpdated;
        }
    }

    public static readonly StyledProperty<BotMemory?> BotMemoryProperty =
        AvaloniaProperty.Register<ChatsPanel, BotMemory?>(nameof(BotMemory), defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<ChatHistory?> SelectedChatProperty =
        AvaloniaProperty.Register<ChatsPanel, ChatHistory?>(nameof(SelectedChat), defaultBindingMode: BindingMode.OneWayToSource);

    public static readonly RoutedEvent<ChatSelectedEventArgs> ChatSelectedEvent =
        RoutedEvent.Register<ChatsPanel, ChatSelectedEventArgs>(nameof(ChatSelected), RoutingStrategies.Bubble);
}