namespace Metagram.Models.Entities;

public sealed class MessageEntity
{
    public int MessageId { get; set; }
    public long TelegramMessageId { get; set; }
    public int BotChatId { get; set; }
    public string? MediaPath { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}