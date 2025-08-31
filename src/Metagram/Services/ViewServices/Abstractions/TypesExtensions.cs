using Metagram.ViewModels.Abstractions;

namespace Metagram.Services.ViewServices.Abstractions;

internal static class TypesExtensions
{
    public static IServiceCollection AddViewModelLocator(this IServiceCollection services, Action<IViewmodelLocatorBuilder> configure)
    {
        ViewmodelLocatorBuilder builder = new ViewmodelLocatorBuilder(services);
        configure.Invoke(builder);

        ViewModelLocator locator = new ViewModelLocator(builder.ModelsMap);
        services.AddSingleton<IViewModelLocator, ViewModelLocator>(_ => locator);
        return services;
    }

    public static IViewmodelLocatorBuilder AddViewModel<V, F>(this IViewmodelLocatorBuilder builder) where V : class, IViewModel<F> where F : FrameworkElement
    {
        builder.ModelsMap.Add(typeof(F), typeof(V));
        builder.Services.AddSingleton<IViewModel<F>, V>();
        return builder;
    }
}
