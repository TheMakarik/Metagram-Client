using System.Collections.ObjectModel;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Collections;

public sealed class ObservableMessageFlow : ObservableCollection<ObservableMessageGroup>
{
}

public sealed class ObservableMessageGroup : ObservableCollection<MessageNode>
{

}

public sealed class MessageNode : ObservableObject
{
    [ObservableProperty]
    private Message? message;

    [ObservableProperty]
    private bool? isFirst;

    [ObservableProperty]
    private bool? isLast;

    [ObservableProperty]
    private bool? isSingle;
}
