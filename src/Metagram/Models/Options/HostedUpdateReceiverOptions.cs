using Telegram.Bot.Types.Enums;

namespace Metagram.Models.Options;

public class HostedUpdateReceiverOptions
{
    public required IEnumerable<UpdateType> AllowedUpdates { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}