namespace Metagram.UnitTests.FactoriesTests;

public class SqliteConnectionFactoryTests : IDisposable
{
    private static readonly string DbPath = $"{Path.GetTempFileName()}.db";
    private static readonly string ConnectionString = $"Data Source={DbPath}";
    
    private readonly IOptions<SqliteOptions> _stubOptions;
    private readonly ILogger<SqliteConnectionFactory> _stubLogger;
    private IDbConnection _connection;

    public SqliteConnectionFactoryTests()
    {
        _stubLogger = A.Dummy<ILogger<SqliteConnectionFactory>>();
        _stubOptions = A.Fake<IOptions<SqliteOptions>>();
        A.CallTo(() => _stubOptions.Value).Returns(new SqliteOptions { ConnectionString = ConnectionString, MessageTypes = [] });
    }
    
    [Fact]
    public void GetFactory_Always_ReturnsConnectionWithConnectStringFromOptions()
    {
        //Arrange
        SqliteConnectionFactory factory = new SqliteConnectionFactory(_stubOptions, _stubLogger);
      
        //Act
        _connection = factory.GetFactory();
       
        //Assert
        Assert.Equal(ConnectionString, _connection.ConnectionString);
    }

    public void Dispose()
    {
        _connection.Dispose();
        File.Delete(DbPath);
    }
}