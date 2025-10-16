using AssetManagement.Domain.Dtos;

namespace AssetManagement.Contract.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(Guid id);
    Task<PageResponse<T>> GetFilterContent<T>(PageRequest paged, IQueryable<T> query);
    Task<T> InsertAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    IQueryable<T> GetQueryable();
    Task BulkInsertAsync(IEnumerable<T> entities);
    Task<ICollection<T>> GetAllAsync();

}
