using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Service;

/// <summary>
/// Author service implementation demonstrating CRUD operations
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext _context;

    public AuthorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Author> CreateAuthorAsync(AuthorCreateDto authorDto)
    {
        var author = new Author() { Name = authorDto.Name, Country = authorDto.Country, };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<List<GetAuthorWithBookCountDto>> GetBookCountPerAuthorAsync()
    {
        var authorWithBookCount = await _context
            .Authors.Select(x => new GetAuthorWithBookCountDto
            {
                Name = x.Name,
                BookCount = x.Books.Count()
            })
            .ToListAsync();

        return authorWithBookCount;
    }

    public async Task<Author?> GetByBookIdAsync(int bookId)
    {
        var author = await _context
            .Authors.Where(a => a.Books.Any(b => b.Id == bookId))
            .FirstOrDefaultAsync();
        if (author == null)
        {
            return null;
        }
        return author;
    }
}
