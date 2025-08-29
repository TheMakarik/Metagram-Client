using Metagram.Services.ViewServices.Abstractions;

namespace Metagram.Services.ViewServices;

internal class ViewmodelLocatorBuilder(IServiceCollection services) : IViewmodelLocatorBuilder
{
    public IServiceCollection Services { get; } = services;
    public IDictionary<Type, Type> ModelsMap { get; } = new Dictionary<Type, Type>();
}
