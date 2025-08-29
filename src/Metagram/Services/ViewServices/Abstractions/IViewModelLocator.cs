namespace Metagram.Services.ViewServices.Abstractions;

internal interface IViewModelLocator
{
    IReadOnlyDictionary<Type, Type> ViewModelMap { get; }
}
