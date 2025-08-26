namespace Metagram.Model.Options;

public sealed class SqliteOptions
{
    public required string ConnectionString { get; init; }
    public required string[] MessageTypes { get; init; }
}