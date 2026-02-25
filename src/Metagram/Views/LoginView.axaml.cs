namespace Metagram.Views;

public partial class LoginView : UserControl, IViewFor
{
    public object? ViewModel
    {
        get => DataContext;
        set => DataContext = value;
    }

    public LoginView()
    {
        InitializeComponent();
    }
}