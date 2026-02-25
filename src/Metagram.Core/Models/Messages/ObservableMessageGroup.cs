namespace Metagram.Models.Messages;

public sealed partial class ObservableMessageGroup(ObservableMessagesFlow flow, User from) : ObservableCollection<MessageNode>
{
    private readonly ObservableMessagesFlow _flow = flow;
    private readonly User _from = from;

    public ObservableMessagesFlow Flow => _flow;
    public User From => _from;
    public Chat Chat => Flow.Chat;
    public string? ChatTitle => Flow.ChatTitle;
    public string? UserTitle => From.Username;

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
            prev.IsFirst = prev.Previous == null;
            prev.IsLast = prev.Next == null;

            item.Previous = prev;
            item.IsFirst = item.Previous == null;
            item.IsLast = item.Next == null;
        }

        if (index < Count)
        {
            MessageNode next = this[index];
            next.Previous = item;
            next.IsFirst = next.Previous == null;
            next.IsLast = next.Next == null;

            item.Next = next;
            item.IsFirst = item.Previous == null;
            item.IsLast = item.Next == null;
        }

        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        MessageNode node = this[index];

        node.Previous?.Next = node.Next;
        node.Next?.Previous = node.Previous;

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
