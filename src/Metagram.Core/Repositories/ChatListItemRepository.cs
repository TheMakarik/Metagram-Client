namespace Metagram.Repositories;

public sealed class ChatListItemRepository(ILogger<ChatListItemRepository> logger, IDbContextFactory<MetagramDbContext> factory) : IChatListItemRepository
{
    public IAsyncEnumerable<ChatListItem> GetChatListAsync()
    {
        using MetagramDbContext dbContext = factory.CreateDbContext();
        return dbContext.Chats.AsNoTracking()
            .Include(chat => chat.Owners)
            .Where(chat => chat.Owners.Any(owner => owner.IsCurrentBot))
            .ToChatListItemDto()
            .Concat(dbContext.Users.Where(user => user.Owner != null && user.Owner.IsCurrentBot && user.Chat == null)
                .ToChatListItemDto())
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<MetagramMessage> GetMessagePaginationAsync(ChatListItem item, int oneBasedStart, int end)
    {
        logger.LogDebug("Loading messages from chat with name = {name} from {start} to {end}...", item.Name, oneBasedStart, end);
     
        Debug.Assert(oneBasedStart <= end);
        ArgumentNullException.ThrowIfNull(item);
        
        using MetagramDbContext dbContext = factory.CreateDbContext();
        return item.IsUser
            ? dbContext.Messages
                .AsNoTracking()
                .Where(message => message.Chat == null)
                .Where(message => message.User != null && message.User.TelegramId == item.TelegramId)
                .Skip(oneBasedStart)
                .Take(end - oneBasedStart)
                .AsAsyncEnumerable()
            : dbContext.Messages
                .AsNoTracking()
                .Where(message => message.User == null)
                .Where(message => message.Chat != null && message.Chat.TelegramId == item.TelegramId)
                .Skip(oneBasedStart)
                .Take(end - oneBasedStart)
                .AsAsyncEnumerable();
    }
}
