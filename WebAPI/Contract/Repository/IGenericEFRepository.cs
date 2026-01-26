namespace Contract.Repository;

public interface IGenericEFRepository<T>
    where T : class
{
    Task<Guid> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<T?> GetByPrimaryKey(Guid id);
}
