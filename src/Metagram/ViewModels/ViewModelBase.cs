namespace Metagram.ViewModels;

public abstract class ViewModelBase() : ReactiveObject;

public abstract class ScreenViewModelBase() : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
}

public abstract class RoutableViewModelBase(string? path, IScreen host) : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = path;
    public IScreen HostScreen { get; } = host;
}
