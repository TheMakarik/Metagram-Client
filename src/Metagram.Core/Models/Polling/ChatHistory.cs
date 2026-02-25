namespace Metagram.Models.Polling;

public class ChatHistory
{
    public Chat Chat { get; }
    public ObservableMessagesFlow Flow { get; }
    public string? Title { get; }

    public ChatHistory(BotRuntimeSession session, Chat chat)
    {
        Chat = chat;
        Flow = new ObservableMessagesFlow(session, chat);
        Title = Chat.ToDisplayString();
    }

    public static ChatHistory FromFirstMessage(BotRuntimeSession session, Message msg)
    {
        return new ChatHistory(session, msg.Chat);
    }
}

public static class ChatHistoryExtensions
{
    public static ChatHistory AddMessage(this ChatHistory chatMemory, Message message)
    {
        if (message.From is not { Id: > 0 } from)
            throw new ArgumentException("Sender must not be null", nameof(message));

        ObservableMessageGroup group = chatMemory.Flow.FindSender(from);
        group.Add(message);
        return chatMemory;
    }
}
