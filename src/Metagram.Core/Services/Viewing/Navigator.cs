namespace Metagram.Services.Viewing;

/*
public class Navigator(IScreen hostScreen, IServiceProvider serviceProvider) : INavigator
{
    private readonly IScreen _hostScreen = hostScreen;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void NavigateTo<TViewModel>() where TViewModel : IRoutableViewModel
    {
        TViewModel vm = _serviceProvider.GetRequiredService<TViewModel>();
        _hostScreen.Router.Navigate.Execute(vm);
    }

    public void GoBack()
    {
        _hostScreen.Router.NavigateBack.Execute();
    }
}
*/
