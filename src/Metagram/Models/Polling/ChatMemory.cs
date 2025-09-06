using Metagram.Collections;
using System.Windows.Interop;
using Chat = Telegram.Bot.Types.Chat;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Models.Polling;

public class ChatMemory
{
    private readonly Chat _chat;
    private readonly ObservableMessagesFlow _flow;
    private readonly string? _title;

    public Chat Chat => _chat;
    public ObservableMessagesFlow Flow => _flow;
    public string? Title => _title;

    public ChatMemory(Chat chat)
    {
        _chat = chat;
        _flow = new ObservableMessagesFlow(chat);
        _title = Chat.ToTitle();
    }

    public static ChatMemory FromFirstMessage(Message msg)
    {
        return new ChatMemory(msg.Chat);
    }
}

public static class ChatMemoryExtensions
{
    public static ChatMemory AddMessage(this ChatMemory chatMemory, Message message)
    {
        if (message.From is not { Id: > 0 } from)
            throw new ArgumentException("Sender must not be null", nameof(message));
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            ObservableMessageGroup group = chatMemory.Flow.FindSender(from);
            group.Add(message);
        }); 

        return chatMemory;
    }
}
