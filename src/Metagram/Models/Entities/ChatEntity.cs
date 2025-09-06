namespace Metagram.Models.Entities;

public sealed class ChatEntity
{
    public int ChatId { get; set; }
    public int BotChatId { get; set; }
    public string? Title { get; set; }
    public long TelegramChatId { get; set; }
    public string? AvatarsPath { get; set; }
    public required string ChatName { get; set; }
}