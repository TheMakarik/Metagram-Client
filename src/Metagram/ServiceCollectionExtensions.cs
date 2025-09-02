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
            .AddSingleton<IUpdateHandler, MetaUpdateHandler>();
    }

    internal static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {
        /* Im not sure about this realization,
         * cuz in telegrator this shit spams me in logs with gc errors.
         * I need to hold a reference on client inside update receiver,
         * but httpclientfactory thinks different

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