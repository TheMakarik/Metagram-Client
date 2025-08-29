
namespace Metagram.IntegrationalTests;

public sealed class DatabaseLifeTime : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        ILogger<DatabaseInitializer> stubLogger = A.Dummy<ILogger<DatabaseInitializer>>();
        IDatabaseInitializer initializer = new DatabaseInitializer(TestSqliteConnectionFactory.Instance, stubLogger);
        await initializer.InitializeAsync();
        //TO DO: create fake database
    }

    public async Task DisposeAsync()
    {
        using IDbConnection dbConnection = TestSqliteConnectionFactory.Instance.GetFactory();
        dbConnection.Open();
    }
}