using LibraryManagement.DTO;
using LibraryManagement.Model;

namespace LibraryManagement.Service;

public interface IBookService
{
    Task<Book?> UpdateBookAsync(int id, BookUpdateDto bookDto);
    Task<IEnumerable<Book>> GetAllWithAuthorByEager();
    Task<IEnumerable<Book>> GetAllWithAuthorByLazy();

    Task<IEnumerable<Book>> GetBorrowedBooksByBorrowerId(int borrowerId);

    Task<IEnumerable<Book>> GetTopThreeBorrowedBooks();
}
