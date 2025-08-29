using Metagram.ViewModels.Abstractions;

namespace Metagram.ViewModels;

public abstract class BaseViewModel<F> : ObservableObject, IViewModel<F> where F : FrameworkElement;