namespace AssetManagementSystem.Contracts.Repositories;

/// <summary>
/// defines database operations
/// </summary>
public interface IGenericRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetAsync(Guid id);

    Task<T> InsertAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> UpdateRangeAsync(IEnumerable<T> entities);

    Task<bool> DeleteAsync(T entity);

    IQueryable<T> GetQueryable();
}
