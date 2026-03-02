#if DEBUG

namespace Metagram.Repositories;

public class DatabaseEditorRepository(IDbContextFactory<MetagramDbContext> dbContextFactory) : IDatabaseEditorRepository
{
    public string[] GetTables()
    {
        using MetagramDbContext dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Model.GetEntityTypes()
            .Select(entityType => entityType.GetTableName() ?? string.Empty)
            .ToArray();
    }

    public string[] GetColumns(string tableName)
    {
        using MetagramDbContext dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Model.GetEntityTypes()
            .First(entityType => entityType.GetTableName() == tableName)
            .GetProperties()
            .Where(property => !(property.IsShadowProperty() || !property.IsForeignKey()))
            .Select(property => property.GetColumnName())
            .ToArray();
    }

    public string[] GetIndexes(string tableName)
    {
        using MetagramDbContext dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Model.GetEntityTypes()
            .First(entityType => entityType.GetTableName() == tableName)
            .GetIndexes()
            .Select(index => index.Name ?? string.Empty)
            .ToArray();
    }
}

#endif