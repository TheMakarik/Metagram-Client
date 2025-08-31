using Metagram.Models.Polling;

namespace Metagram.Services.PollingServices.Abstractions;

internal static class TypesExtensions
{
    public static IBotMemory AddUpdate(this IBotMemory botMemory, Update update)
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

    public static IBotMemory AddMessage(this IBotMemory botMemory, Message message)
    {
        if (!botMemory.TryGetChat(message.Chat.Id, out ChatMemory? chatMemory))
        {
            chatMemory = new ChatMemory(message);
            botMemory.AddChat(chatMemory);
        }

        chatMemory.AddMessage(message);
        return botMemory;
    }
}
