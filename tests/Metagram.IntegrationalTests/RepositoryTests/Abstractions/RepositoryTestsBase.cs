namespace Metagram.IntegrationalTests.RepositoryTests.Abstractions;

public abstract class RepositoryTestBase : IAsyncLifetime
{
    private readonly DatabaseFixture _databaseLifeTime;
    
    protected readonly IDbConnection DbConnection;

    protected RepositoryTestBase()
    {
        _databaseLifeTime = new DatabaseFixture();
        DbConnection = TestSqliteConnectionFactory.Instance.GetFactory();
    }

    public async Task InitializeAsync()
    {
        await _databaseLifeTime.InitializeAsync();
        DbConnection.Open();
    }

    public async Task DisposeAsync()
    {
        DbConnection.Close();
        await _databaseLifeTime.DisposeAsync();
    }

}