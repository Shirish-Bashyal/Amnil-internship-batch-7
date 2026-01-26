using LibraryManagement.DTO;
using LibraryManagement.Model;

namespace LibraryManagement.Service;

public interface IBorrowService
{
    Task<IEnumerable<Borrower>> GetAllBorrowersAsync();
    Task<bool> DeleteBorrowerAsync(int id);

    Task<List<GetBorrowersGroupByBookCountDto>> GetBorrowersGroupByBookCount();
}
