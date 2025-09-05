using Metagram.Services.PollingServices.Abstractions;
using System.Windows.Interop;
using Chat = Telegram.Bot.Types.Chat;

namespace Metagram;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfigurationManager configuration)
    {
        return services
            .Configure<HostedUpdateReceiverOptions>(configuration.GetSection(nameof(HostedUpdateReceiverOptions)))
            .Configure<SqliteOptions>(configuration.GetSection(nameof(SqliteOptions)));
    }
    
    internal static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services) where THostedService : class, IHostedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
        return services;
    }

    internal static IServiceCollection AddPolling(this IServiceCollection services)
    {
        return services
            .AddHostedService<HostedUpdateReceiver>()
            .AddSingleton<IUpdateHandler, MetaUpdateHandler>()
            .AddSingleton<IBotMemory, BotMemory>();
    }

    internal static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {
        /*
        services.AddHttpClient<ITelegramBotClient>("tgreceiver").RemoveAllLoggers().AddTypedClient((httpClient, provider)
            => new TelegramBotClient(provider.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value, httpClient));
        */

        services.AddTransient<ITelegramBotClient, TelegramBotClient>(serviceProvider => new TelegramBotClient(serviceProvider.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value));
        return services;
    }
    
    public static IServiceCollection AddViewModelLocator(this IServiceCollection services, Action<IViewmodelLocatorBuilder> configure)
    {
        ViewmodelLocatorBuilder builder = new ViewmodelLocatorBuilder(services);
        configure.Invoke(builder);

        ViewModelLocator locator = new ViewModelLocator(builder.ModelsMap);
        services.AddSingleton<IViewModelLocator, ViewModelLocator>(_ => locator);
        return services;
    }
}

public static class LinkedListExtensions
{
    public static LinkedList<T> Add<T>(this LinkedList<T> list, T element)
    {
        list.AddLast(element);
        return list;
    }
}

public static class TelegramBotTypesExtensions
{
    public static string? ToTitle(this Chat chat)
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