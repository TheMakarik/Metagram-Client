using System.Collections.ObjectModel;
using Chat = Telegram.Bot.Types.Chat;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Models.Polling;

public class ChatMemory : DispatcherObject
{
    public Chat Chat { get; set; }
    public ObservableCollection<KeyValuePair<long, LinkedList<Message>>> Messages { get; set; } = [];
    public string? ChatTitle { get; set; }

    public ChatMemory(Message msg)
    {
        Chat = msg.Chat;
        string?[] strings = [Chat.Title, Chat.FirstName, Chat.LastName, Chat.Username];
        ChatTitle = strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));
    }

    public void AddMessage(Message msg)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (msg.From is not { Id: > 0 } from)
                return;

            KeyValuePair<long, LinkedList<Message>> msgGroup = Messages.Last();
            if (msgGroup.Key != from.Id)
                msgGroup = new KeyValuePair<long, LinkedList<Message>>(from.Id, []);

            msgGroup.Value.AddLast(msg);
        });
    }
}
