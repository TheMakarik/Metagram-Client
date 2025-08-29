namespace Metagram.Models.Entities;

public sealed class Chat
{
    public int ChatId { get; set; }
    public int BotChatId { get; set; }
    public string? Title { get; set; }
    public long TelegramChatId { get; set; }
    public required string ChatName { get; set; }
    public int ChatTypeId { get; set; }
    
}