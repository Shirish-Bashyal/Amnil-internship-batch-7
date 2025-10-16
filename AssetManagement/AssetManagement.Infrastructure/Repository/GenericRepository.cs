using AssetManagement.Contract.Repository;
using AssetManagement.Domain.Dtos;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _db;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _db = _context.Set<T>();
    }
    public async Task<ICollection<T>> GetAllAsync()
    {
        return await _db.ToListAsync();
    }

    public async Task BulkInsertAsync(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity= await _db.FindAsync(id);
        if (entity == null)
        {
            return false;
        }
        _db.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _db.FindAsync(id);
    }

    public async Task<PageResponse<T>> GetFilterContent<T>(PageRequest paged, IQueryable<T> query)
    {
        var totalCount = await query.CountAsync();
        var item = await query.Skip((paged.SkipPageCount - 1) * paged.ListCount).Take(paged.ListCount).ToListAsync();
        return new PageResponse<T>
        {
            Items = item,
            TotalCount = totalCount
        };
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

    public async Task<bool> UpdateAsync(T entity)
    {
        _db.Update(entity);

        return await _context.SaveChangesAsync() > 0;
    }
}
