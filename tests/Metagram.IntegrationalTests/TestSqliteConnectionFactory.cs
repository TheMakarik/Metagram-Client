namespace Metagram.IntegrationalTests;

public class TestSqliteConnectionFactory : ISqliteConnectionFactory
{
    public static string DatabaseName { get; } = ":memory:";
    private static readonly string ConnectionString = $"Data Source={DatabaseName};Cache=Shared"; //Do not replace with DataBaseName
  
    private readonly object Lock = new object();
    
    public static TestSqliteConnectionFactory Instance { get; } = new TestSqliteConnectionFactory();
    
    public IDbConnection GetFactory()
    {
        lock (Lock)
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}