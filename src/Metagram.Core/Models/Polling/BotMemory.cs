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


