using System.Threading;

namespace Metagram;

internal static class ServiceCollectionExtensions
{
    public static TService GetRequiredService<TService>(this IServiceProvider serviceProvider, params object[] args)
    {
        return ActivatorUtilities.CreateInstance<TService>(serviceProvider, args);
    }
}

public static class ScreenExtensions
{
    public static void NavigateTo<TViewModel>(this IScreen screen) where TViewModel : IRoutableViewModel
    {
        TViewModel vm = App.Services.GetRequiredService<TViewModel>();
        screen.Router.Navigate.Execute(vm);
    }

    public static void NavigateBack(this IScreen screen)
    {
        screen.Router.NavigateBack.Execute();
    }
}

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