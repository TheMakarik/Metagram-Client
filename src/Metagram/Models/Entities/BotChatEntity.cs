namespace Metagram.Models.Entities;

public sealed class BotChatEntity
{
    public int BotChatId { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    public required string LastContent { get; set; }
}