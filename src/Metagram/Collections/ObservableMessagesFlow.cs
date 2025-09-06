using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Chat = Telegram.Bot.Types.Chat;
using Message = Telegram.Bot.Types.Message;
using User = Telegram.Bot.Types.User;

namespace Metagram.Collections;

public sealed class ObservableMessagesFlow(Chat chat) : ObservableCollection<ObservableMessageGroup>
{
    public Chat Chat { get; } = chat;
    public string? ChatTitle { get; } = chat.ToTitle();
    public int FullCount => this.Select(c => c.Count).Sum();

    public ObservableMessageGroup FindSender(User sender)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        ObservableMessageGroup? group = this.LastOrDefault();
        if (group == null || group.From.Id != sender.Id)
        {
            group = new ObservableMessageGroup(this, sender);
            Add(group);
        }

        return group;
    }
}

public sealed class ObservableMessageGroup(ObservableMessagesFlow flow, User from) : ObservableCollection<MessageNode>
{
    private readonly ObservableMessagesFlow _flow = flow;
    private readonly User _from = from;

    public ObservableMessagesFlow Flow => _flow;
    public User From => _from;
    public Chat Chat => Flow.Chat;
    public string? ChatTitle => Flow.ChatTitle;
        
    public MessageNode? First => this.FirstOrDefault();
    public MessageNode? Last => this.LastOrDefault();

    public void Add(Message message)
    {
        MessageNode node = new MessageNode(this, message);
        Add(node);
    }

    protected override void InsertItem(int index, MessageNode item)
    {
        if (index > 0)
        {
            MessageNode prev = this[index - 1];
            prev.Next = item;
            item.Previous = prev;
        }

        if (index < Count)
        {
            MessageNode next = this[index];
            next.Previous = item;
            item.Next = next;
        }

        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        MessageNode node = this[index];

        if (node.Previous != null)
            node.Previous.Next = node.Next;

        if (node.Next != null)
            node.Next.Previous = node.Previous;

        node.Next = null;
        node.Previous = null;

        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        foreach (MessageNode node in this)
            node.Dispose();

        base.ClearItems();
    }
}

public sealed partial class MessageNode : ObservableObject, IDisposable
{
    private readonly ObservableMessageGroup _group;

    private bool _deleted; // aka _dispoded

    public ObservableMessageGroup Group => _group;
    public Chat Chat => Group.Chat;
    public User From => Group.From;
    public string? ChatTitle => Group.Flow.ChatTitle;

    public MessageNode? Previous { get; set; }
    public MessageNode? Next { get; set; }

    [ObservableProperty]
    private Message? message;

    [ObservableProperty]
    private bool isFirst = false;

    [ObservableProperty]
    private bool isLast = false;

    [ObservableProperty]
    private bool isSingle = false;

    public MessageNode(ObservableMessageGroup group, Message initMessage)
    {
        _group = group;
        message = initMessage;
        _group.CollectionChanged += GroupChanged;
    }

    private void GroupChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        IsSingle = Group.Count == 1;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.PropertyName)
        {
            case nameof(Message):
                {

                    break;
                }

            case nameof(Previous):
                {
                    if (!IsFirst && Previous == null)
                    {
                        IsFirst = true;
                        break;
                    }

                    if (IsFirst && Previous != null)
                    {
                        IsFirst = false;
                        break;
                    }

                    IsFirst = Previous == null;
                    break;
                }

            case nameof(Next):
                {
                    if (!IsLast && Next == null)
                    {
                        IsLast = true;
                        break;
                    }

                    if (IsLast && Next != null)
                    {
                        IsLast = false;
                        break;
                    }

                    IsLast = Next == null;
                    break;
                }
        }
    }

    public void Dispose()
    {
        if (_deleted)
            return;

        Previous = null;
        Next = null;
        Message = null;

        _deleted = true;
        GC.SuppressFinalize(this);
    }

    public override string ToString() => Message?.Text ?? "DEBUG_WARN:NULL_NODE_VALUE";
}
