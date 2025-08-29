namespace Metagram.Services.ViewServices.Abstractions;

internal interface IViewmodelLocatorBuilder
{
    IServiceCollection Services { get; }
    IDictionary<Type, Type> ModelsMap { get; }
}
