namespace Metagram.Factories;

public class SqliteConnectionFactory(
    IOptions<SqliteOptions> dbOptions,
    ILogger<SqliteConnectionFactory> logger
    ) : ISqliteConnectionFactory
{
    private const string ConnectingToSqliteDatabaseLogMessage = "Connection to sqlite database with connection string: \"{connectionString}\"";
    
    public IDbConnection GetFactory()
    {
        string connectionString = Environment.ExpandEnvironmentVariables(dbOptions.Value.ConnectionString);
        logger.LogDebug(ConnectingToSqliteDatabaseLogMessage, connectionString);
        return new SqliteConnection(connectionString);
    }
}