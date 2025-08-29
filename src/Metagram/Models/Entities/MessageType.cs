namespace Metagram.Models.Entities;

public sealed class MessageType
{
    public int MessageTypeId { get; set; }
    public required string Name { get; set; }
}