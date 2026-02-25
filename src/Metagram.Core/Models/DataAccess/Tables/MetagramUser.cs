namespace Metagram.Models.DataAccess.Tables;

public sealed class MetagramUser
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public MetagramFile? Avatar { get; set; }
    public Chat? Chat { get; set; }
    public ICollection<MetagramMessage> Messages { get; set; }
    public MetagramBot? Owner { get; set; }
}
