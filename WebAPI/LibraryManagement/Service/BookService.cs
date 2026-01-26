using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Service;

/// <summary>
/// Book service implementation demonstrating CRUD operations relating to book entity
/// </summary>
public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllWithAuthorByEager()
    {
        var books = await _context.Books.Include(x => x.Author).ToListAsync();
        return books;
    }

    public async Task<IEnumerable<Book>> GetAllWithAuthorByLazy()
    {
        var books = await _context.Books.ToListAsync();
        foreach (var book in books)
        {
            _ = book.Author;
        }

        return books;
    }

    public async Task<IEnumerable<Book>> GetBorrowedBooksByBorrowerId(int borrowerId)
    {
        var books = await _context
            .Books.Where(b => b.BorrowerBooks.Any(bb => bb.BorrowerId == borrowerId))
            .ToListAsync();
        return books;
    }

    public async Task<IEnumerable<Book>> GetTopThreeBorrowedBooks()
    {
        var topBorrowedBooks = await _context
            .Books.OrderByDescending(x => x.BorrowerBooks.Count())
            .Take(3)
            .ToListAsync();
        return topBorrowedBooks;
    }

    public async Task<Book?> UpdateBookAsync(int id, BookUpdateDto bookDto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return null;
        }

        var author = await _context.Authors.FindAsync(bookDto.AuthorId);
        if (author == null)
        {
            return null;
        }

        book.Title = bookDto.Title;
        book.ISBN = bookDto.ISBN;
        book.PublishedYear = bookDto.PublishedYear;
        book.Author = author;

        await _context.SaveChangesAsync();
        return book;
    }
}
