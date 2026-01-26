using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Service;

/// <summary>
///Borrower service implementation demonstrating CRUD operations
/// </summary>
public class BorrowerService : IBorrowService
{
    private readonly ApplicationDbContext _context;

    public BorrowerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteBorrowerAsync(int id)
    {
        var borrower = await _context.Borrowers.FindAsync(id);
        if (borrower == null)
        {
            return false;
        }

        _context.Borrowers.Remove(borrower);
        return true;
    }

    public async Task<IEnumerable<Borrower>> GetAllBorrowersAsync()
    {
        var borrowers = await _context.Borrowers.AsNoTracking().ToListAsync();
        return borrowers;
    }

    public async Task<List<GetBorrowersGroupByBookCountDto>> GetBorrowersGroupByBookCount()
    {
        var bookCountWithBorrowers = await _context
            .Borrowers.Select(x => new { Borrower = x, BorrowedCount = x.BorrowerBooks.Count() })
            .ToListAsync();
        var borrowersGroupedByBookCount = bookCountWithBorrowers
            .GroupBy(x => x.BorrowedCount)
            .Select(x => new GetBorrowersGroupByBookCountDto
            {
                BorrowedBookCount = x.Key,
                Borrowers = x.Select(x => x.Borrower).ToList()
            })
            .ToList();

        return borrowersGroupedByBookCount;
    }
}
