using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace AssetManagementSystem.Infrastructure.Repositories;

public class GenericEfRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _db;

    public GenericEfRepository(ApplicationDbContext context)
    {
        _context = context;
        _db = _context.Set<T>();
    }
   
    public async Task<T> CreateAsync(T entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)//just changes is active
    {
        
        _db.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _db.FindAsync(id);
    }

    public  IQueryable<T> GetQueryable()
    {
        return _db;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _db.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}
