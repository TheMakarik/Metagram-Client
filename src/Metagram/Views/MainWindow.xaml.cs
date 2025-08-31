using Metagram.ViewModels.Abstractions;

namespace Metagram.Views;

public partial class MainWindow
{
    public MainWindow(IViewModel<MainWindow> viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}