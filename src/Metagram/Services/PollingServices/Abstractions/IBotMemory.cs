using Metagram.Models.Polling;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Metagram.Services.PollingServices.Abstractions;

public interface IBotMemory
{
    public ObservableCollection<ChatMemory> Chats { get; }

    public bool TryGetChat(long id, [NotNullWhen(true)] out ChatMemory? chatMemory);
    public void AddChat(ChatMemory chat);
}
