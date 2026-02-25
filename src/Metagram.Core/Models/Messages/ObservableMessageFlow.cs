namespace Metagram.Models.Messages;

public sealed partial class ObservableMessagesFlow(BotRuntimeSession session, Chat chat) : ObservableCollection<ObservableMessageGroup>
{
    public BotRuntimeSession Session { get; } = session;
    public Chat Chat { get; } = chat;

    public string? ChatTitle => Chat.ToDisplayString();
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
