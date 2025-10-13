using AssetManagement.Contract.Repository;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T:class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _db;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _db = _context.Set<T>();
    }
    public async Task<bool> DeleteAsync(T entity)
    {
       _db.Remove(entity);
        return await _context.SaveChangesAsync()>0;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _db.FindAsync(id);
    }

    public IQueryable<T> GetQueryable()
    {
        return _db;
    }

    public async Task<T> InsertAsync(T entity)
    {
        await _db.AddAsync(entity); 
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _db.Update(entity);
        await _context.SaveChangesAsync();
        return entity;  
    }
}
