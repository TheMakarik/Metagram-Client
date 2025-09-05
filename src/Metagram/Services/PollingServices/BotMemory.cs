using Metagram.Models.Polling;
using Metagram.Services.PollingServices.Abstractions;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using User = Telegram.Bot.Types.User;

namespace Metagram.Services.PollingServices;

internal class BotMemory : DispatcherObject, IBotMemory
{
    private readonly Dictionary<long, ChatMemory> _chatsDictionary = [];

    public User MyUser { get; }
    public ObservableCollection<ChatMemory> Chats { get; } = [];

    public BotMemory(ITelegramBotClient botClient)
    {
        MyUser = botClient.GetMe().Result;
    }

    public bool TryGetChat(long id, [NotNullWhen(true)] out ChatMemory? chatMemory)
    {
        return _chatsDictionary.TryGetValue(id, out chatMemory);
    }

    public void AddChat(ChatMemory chatMemory)
    {
        _chatsDictionary.Add(chatMemory.Chat.Id, chatMemory);
        Application.Current.Dispatcher.Invoke(() => Chats.Add(chatMemory));
    }
}
