using LibraryManagement.DTO;
using LibraryManagement.Model;

namespace LibraryManagement.Service;

public interface IAuthorService
{
    Task<Author> CreateAuthorAsync(AuthorCreateDto authorDto);

    Task<Author?> GetByBookIdAsync(int bookId);

    Task<List<GetAuthorWithBookCountDto>> GetBookCountPerAuthorAsync();
}
