using System.Collections.Specialized;

namespace Metagram.Models.Messages;

public sealed partial class MessageNode : ReactiveObject, IDisposable
{
    private bool disposed;

    public ObservableMessageGroup Group { get; }
    public Chat Chat => Group.Chat;
    public User From => Group.From;

    public string? UserTitle => From.ToDisplayString();

    [Reactive]
    private MessageNode? previous;

    [Reactive]
    private MessageNode? next;

    [Reactive]
    private Message? message;

    [Reactive]
    private bool isFirst = false;

    [Reactive]
    private bool isLast = false;

    [Reactive]
    private bool isSingle = false;

    [Reactive]
    private bool isMyMessage = false;

    public MessageNode(ObservableMessageGroup group, Message initMessage)
    {
        Message = initMessage;
        Group = group;
        Group.CollectionChanged += GroupChanged;

        IsMyMessage = initMessage.From?.IsBot ?? false;
    }

    private void GroupChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        IsSingle = Group.Count == 1;
    }

    public void Dispose()
    {
        if (disposed)
            return;

        Previous = null;
        Next = null;
        Message = null;

        disposed = true;
        GC.SuppressFinalize(this);
    }

    public override string ToString() => Message?.Text ?? "{NULL_MESSAGE_VALUE}";
}
