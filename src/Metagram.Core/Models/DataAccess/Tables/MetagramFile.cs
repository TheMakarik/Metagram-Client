namespace Metagram.Models.DataAccess.Tables;

public class MetagramFile
{
    public int Id { get; set; }
    public string Path { get; set; }
    public long TelegramId { get; set; }
}
