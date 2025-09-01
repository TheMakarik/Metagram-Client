namespace Metagram.Models.Repositories.Abstractions;

public interface IRepositoryBase<T>
{
    public Task<T> GetAsync(int id);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(int id);
    public Task CreateAsync(T entity);
}