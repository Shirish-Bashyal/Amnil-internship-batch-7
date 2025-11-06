using AssetManagementSystem.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Contract.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(Guid id);

    Task<User> GetByUserNameAsync(string name);
    IQueryable<T> GetQueryable();

    Task<IEnumerable<T>> GetAllAsync();

    Task<T> CreateAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(T entity);
    Task InsertRangeAsync(IEnumerable<T> entities);
}
