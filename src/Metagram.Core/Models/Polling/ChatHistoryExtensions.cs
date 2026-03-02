namespace Metagram.Models.Polling;

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
