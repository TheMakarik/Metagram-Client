using Metagram.Models.DataAccess;

namespace Metagram.Models.DataAccess.Tables;

public sealed class MetagramMessage 
{
    public long Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Text { get; set; }
    public MetagramChat? Chat { get; set; }
    public MetagramUser? User { get; set; }
    public MetagramFile? Avatar { get; set; }
    public MetagramMessageType Type { get; set; }
    
}
