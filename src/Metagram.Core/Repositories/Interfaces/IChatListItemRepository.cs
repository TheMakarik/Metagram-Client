namespace Metagram.Repositories.Interfaces;

public interface IChatListItemRepository
{
    public IAsyncEnumerable<ChatListItem> GetChatListAsync();
    public IAsyncEnumerable<MetagramMessage> GetMessagePaginationAsync(ChatListItem item, int oneBasedStart, int end);
}
