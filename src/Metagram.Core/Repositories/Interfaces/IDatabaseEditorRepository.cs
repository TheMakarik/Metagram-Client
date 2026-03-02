#if DEBUG

namespace Metagram.Repositories.Interfaces;

public interface IDatabaseEditorRepository
{
    public string[] GetTables();
    public string[] GetColumns(string tableName);
    public string[] GetIndexes(string tableName);
}

#endif