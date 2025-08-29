using Metagram.Services.ViewServices.Abstractions;
using System.Collections.ObjectModel;

namespace Metagram.Services.ViewServices;

internal class ViewModelLocator(IDictionary<Type, Type> viewModelMap) : IViewModelLocator
{
    public IReadOnlyDictionary<Type, Type> ViewModelMap { get; } = new ReadOnlyDictionary<Type, Type>(viewModelMap);
}
