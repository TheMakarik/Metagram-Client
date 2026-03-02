namespace Metagram.Extensions;

public static class BotMemoryExtensions
{
    public static BotMemory AddUpdate(this BotMemory botMemory, BotRuntimeSession session, Update update)
    {
        switch (update)
        {
            case { Message: { } message }:
            {
                botMemory.AddMessage(session, message);
                break;
            }
        }

        return botMemory;
    }

    public static BotMemory AddMessage(this BotMemory botMemory, BotRuntimeSession session, Message message)
    {
        if (!botMemory.TryGetChat(message.Chat.Id, out ChatHistory? chatMemory))
            chatMemory = botMemory.AddChat(session, message.Chat);

        chatMemory.AddMessage(message);
        return botMemory;
    }

    public static ChatHistory AddChat(this BotMemory botMemory, BotRuntimeSession session, Chat chat)
    {
        ChatHistory chatMemory = new ChatHistory(session, chat);
        botMemory.AddChat(chatMemory);
        return chatMemory;
    }
}