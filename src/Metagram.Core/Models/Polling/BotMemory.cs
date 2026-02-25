using Metagram.Models.Polling;
using System.Diagnostics.CodeAnalysis;

namespace Metagram.Models.Polling;

public sealed class BotMemory
{
    private readonly Dictionary<long, ChatHistory> _chatsMap;

    public ObservableCollection<ChatHistory> Chats { get; }

    public BotMemory()
    {
        _chatsMap = [];
        Chats = [];
    }

    public bool TryGetChat(long chatId, [NotNullWhen(true)] out ChatHistory? chatMemory)
    {
        return _chatsMap.TryGetValue(chatId, out chatMemory);
    }

    public void AddChat(ChatHistory chatMemory)
    {
        _chatsMap.Add(chatMemory.Chat.Id, chatMemory);
        Chats.Add(chatMemory);
    }
}

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
