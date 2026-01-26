using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Contracts.Repository;

public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetAsync(Guid id);

    IQueryable<T> GetQueryable();
    Task<IEnumerable<T>> GetAllAsync();

    // Task<PagedResponse<GetAuthorDto>> GetListAsync(int page, int pageSize);

    Task<T> InsertAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(T entity);
}
