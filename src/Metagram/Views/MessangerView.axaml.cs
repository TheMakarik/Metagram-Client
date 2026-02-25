using Avalonia.Controls;
using Metagram.ViewModels;

namespace Metagram.Views;

public partial class MessangerView : UserControl, IViewFor
{
    public object? ViewModel
    {
        get => DataContext;
        set => DataContext = value;
    }

    public MessangerView()
    {
        InitializeComponent();
    }
}