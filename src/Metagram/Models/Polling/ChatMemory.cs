using System.Collections.ObjectModel;

namespace Metagram.Models.Polling;

public class ChatMemory : DispatcherObject
{
    public Chat Chat { get; set; }
    public ObservableCollection<Message> Messages { get; set; } = [];
    public string? ChatTitle { get; set; }

    public ChatMemory(Message msg)
    {
        Chat = msg.Chat;
        string?[] strings = [Chat.Title, Chat.FirstName, Chat.LastName, Chat.Username];
        ChatTitle = strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));
    }

    public void AddMessage(Message msg)
    {
        Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
    }
}
