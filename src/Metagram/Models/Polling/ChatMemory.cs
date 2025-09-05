using Metagram.Collections;
using Chat = Telegram.Bot.Types.Chat;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Models.Polling;

/*
public class ChatMemory : DispatcherObject
{
    private readonly Chat _chat;
    private readonly ObservableMessagesFlow _flow;

    public Chat Chat => _chat;
    public ObservableMessagesFlow Flow => _flow;
    public string? ChatTitle { get; set; }

    private ChatMemory(Chat chat)
    {
        _chat = chat;
        _flow = new ObservableMessagesFlow(chat);

        Chat = msg.Chat;
        ChatTitle = Chat.ToTitle();
    }

    public static ChatMemory FromFirstMessage(Message msg)
    {

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
*/
