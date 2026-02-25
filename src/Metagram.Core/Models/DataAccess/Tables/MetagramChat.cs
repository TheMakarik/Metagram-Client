namespace Metagram.Models.DataAccess.Tables;

public class MetagramChat
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public MetagramFile? Avatar { get; set; }
    public required ICollection<MetagramBot> Owners { get; set; }
    public required ICollection<MetagramMessage> Messages { get; set; }
}
