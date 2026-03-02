namespace Metagram.Extensions;

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

