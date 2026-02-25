namespace Metagram.Models.Options;

public class HostedUpdateReceiverOptions
{
    public bool DropPendingUpdates { get; set; }
    public IEnumerable<UpdateType>? AllowedUpdates { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}
