using Metagram.Services.PollingServices;
using Metagram.Services.PollingServices.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Metagram.Services;

internal static class TypesExtensions
{
    public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services) where THostedService : class, IHostedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
        return services;
    }

    public static IServiceCollection AddPolling(this IServiceCollection services)
    {
        return services
            .AddHostedService<HostedUpdateReceiver>()
            .AddSingleton<IUpdateHandler, MetaUpdateHandler>()
            .AddSingleton<IBotMemory, BotMemory>();
    }

    public static IServiceCollection AddTeletgramBot(this IServiceCollection services)
    {
        /* Im not sure about this realization,
         * cuz in telegrator this shit spams me in logs with gc errors.
         * I need to hold a reference on client inside update receiver,
         * but httpclientfactory thinks different
        
        services.AddHttpClient<ITelegramBotClient>("tgreceiver").RemoveAllLoggers().AddTypedClient((httpClient, provider)
            => new TelegramBotClient(provider.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value, httpClient));
        */

        services.AddTransient<ITelegramBotClient, TelegramBotClient>(services => new TelegramBotClient(services.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value));
        return services;
    }
}
