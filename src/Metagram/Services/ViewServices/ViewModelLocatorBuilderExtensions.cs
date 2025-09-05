namespace Metagram.Services.ViewServices;

internal static class ViewModelLocatorBuilderExtensions
{
    public static IViewmodelLocatorBuilder AddViewModel<TView, TFrameworkElement>(this IViewmodelLocatorBuilder builder) where TView : class, IViewModel<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        builder.ModelsMap.Add(typeof(TFrameworkElement), typeof(TView));
        builder.Services.AddSingleton<IViewModel<TFrameworkElement>, TView>();
        return builder;
    }
}
