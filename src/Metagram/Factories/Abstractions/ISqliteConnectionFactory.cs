namespace Metagram.Factories.Abstractions;

public interface ISqliteConnectionFactory
{
    public IDbConnection GetFactory();
}