namespace Metagram.Services;

public static class TelegramBotTypesExtensions
{
    public static string? ToDisplayString(this User user)
    {
        if (!string.IsNullOrEmpty(user.FirstName))
        {
            if (!string.IsNullOrEmpty(user.LastName))
                return user.FirstName + " " + user.LastName;

            return user.FirstName;
        }

        if (!string.IsNullOrEmpty(user.Username))
            return user.Username;

        return null;
    }

    public static string? ToDisplayString(this Chat chat)
    {
        if (!string.IsNullOrEmpty(chat.Title))
            return chat.Title;

        if (!string.IsNullOrEmpty(chat.FirstName))
        {
            if (!string.IsNullOrEmpty(chat.LastName))
                return chat.FirstName + " " + chat.LastName;

            return chat.FirstName;
        }

        if (!string.IsNullOrEmpty(chat.Username))
            return chat.Username;

        return null;
    }
}

public static class CancellationTokenExtensions
{
    public static CancellationToken LinkWith(this CancellationToken cancellationToken, params CancellationToken[] cancellationTokens)
        => CancellationTokenSource.CreateLinkedTokenSource([cancellationToken, .. cancellationTokens]).Token;
}

public static class ExceptionExtensions
{
    public static Exception Aggregate(this Exception exception)
        => exception.InnerException != null ? exception.InnerException.Aggregate() : exception;
}
