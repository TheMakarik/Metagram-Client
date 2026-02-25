namespace Metagram.Models.DataAccess.Dto;

public partial record ChatListItem(long TelegramId, string Name, bool IsUser, MetagramFile? Avatar, MetagramMessage? LastMessage);
