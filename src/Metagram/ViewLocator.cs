namespace Metagram;

public class ViewLocator : IDataTemplate, IViewLocator
{
    public Control? Build(object? data)
    {
        return (Control?)ResolveView(data);
    }

    public bool Match(object? data)
    {
        return data is ReactiveObject;
    }

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is null)
            return null;

        string name = viewModel.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = Type.GetType(name);
        App.Services.GetRequiredService<ILogger<ViewLocator>>().LogDebug("Resolve {view} and {viewmodel}", name, viewModel);

        if (type == null)
            return null;

        object? serviceView = App.Services.GetService(type) ?? Activator.CreateInstance(type);
        if (serviceView is IViewFor view)
            return view;

        return null;
    }
}
