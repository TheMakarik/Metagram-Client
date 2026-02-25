namespace Metagram.Models.DataAccess.Tables;

public sealed class MetagramBot
{
    public long Id { get; set; }
    public required string TelegramToken { get; set; }
    public long TelegramId { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public int AvatarId { get; set; }
    public bool IsCurrentBot { get; set; }
}
