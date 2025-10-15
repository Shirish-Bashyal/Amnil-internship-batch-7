using AssetManagement.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Repository;

public interface IGenericRepository<T>where T : class
{
    Task<T?> GetAsync(Guid id);
    Task<PageResponse<T>> GetFilterContent<T>(PageRequest paged, IQueryable<T> query);
    Task<T> InsertAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    IQueryable<T> GetQueryable();
    Task BulkInsertAsync(IEnumerable<T> entities);

}
