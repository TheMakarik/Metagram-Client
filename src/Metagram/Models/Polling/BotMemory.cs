using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Metagram.Models.Polling;

public sealed class BotMemory : DispatcherObject
{
    private readonly ObservableCollection<ChatMemory> _chats;
    private readonly Dictionary<long, ChatMemory> _chatsMap;

    public ObservableCollection<ChatMemory> Chats => _chats;

    public BotMemory()
    {
        _chats = [];
        _chatsMap = [];
    }

    public bool TryGetChat(long chatId, [NotNullWhen(true)] out ChatMemory? chatMemory)
    {
        return _chatsMap.TryGetValue(chatId, out chatMemory);
    }

    public void AddChat(ChatMemory chatMemory)
    {
        _chatsMap.Add(chatMemory.Chat.Id, chatMemory);
        Application.Current.Dispatcher.Invoke(() => _chats.Add(chatMemory));
    }
}

public static class BotMemoryExtensions
{
    public static BotMemory AddUpdate(this BotMemory botMemory, Update update)
    {
        switch (update)
        {
            case { Message: { } message }:
                {
                    botMemory.AddMessage(message);
                    break;
                }
        }

        return botMemory;
    }

    public static BotMemory AddMessage(this BotMemory botMemory, Message message)
    {
        if (!botMemory.TryGetChat(message.Chat.Id, out ChatMemory? chatMemory))
            chatMemory = botMemory.AddChat(message.Chat);

        chatMemory.AddMessage(message);
        return botMemory;
    }

    public static ChatMemory AddChat(this BotMemory botMemory, Chat chat)
    {
        ChatMemory chatMemory = new ChatMemory(chat);
        botMemory.AddChat(chatMemory);
        return chatMemory;
    }
}