using Metagram.ViewModels.Abstractions;

namespace Metagram.ViewModels;

public abstract class BaseViewModel<TFrameworkElement> : ObservableObject, IViewModel<TFrameworkElement> where TFrameworkElement : FrameworkElement;