namespace Metagram.IntegrationalTests;

public class TestSqliteConnectionFactory : ISqliteConnectionFactory
{
    private static readonly string DatabaseName = Path.GetTempFileName() + ".db";
    private static readonly string ConnectionString = $"Data Source={DatabaseName}";
    
    public static TestSqliteConnectionFactory Instance { get; } = new TestSqliteConnectionFactory();
    
    public IDbConnection GetFactory()
    {
        return new SqliteConnection(ConnectionString);
    }
}