namespace Metagram.Models.DataAccess.MappingExtensions;

public static class MetagramChatListItemExtensions
{
    public static IQueryable<ChatListItem> ToChatListItemDto(this IQueryable<MetagramChat> query)
    {
        return query.Select(chat =>
            new ChatListItem(chat.TelegramId, chat.Name, 
                IsUser: false,
                chat.Avatar,
                chat.Messages.OrderByDescending(message => message.CreatedAt).FirstOrDefault()));
    }

    public static IQueryable<ChatListItem> ToChatListItemDto(this IQueryable<MetagramUser> query)
    {
        return query.Select(user => new ChatListItem(user.TelegramId, string.Concat(user.FirstName," ", user.LastName), 
            IsUser: true,
            user.Avatar,
            user.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()));
    }
}
