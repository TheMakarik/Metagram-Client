namespace Metagram.Services.AppDataServices.Abstractions;

public interface IDatabaseInitializer
{
    public Task InitializeAsync();
}