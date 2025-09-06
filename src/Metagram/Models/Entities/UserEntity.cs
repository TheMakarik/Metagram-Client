namespace Metagram.Models.Entities;

public sealed class UserEntity
{
    public int UserId { get; set; }
    public int BotChatId { get; set; }
    public long TelegramUserId { get; set; }
    public string? Username { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BioText { get; set; }
    public string? AvatarsPath { get; set; }
}