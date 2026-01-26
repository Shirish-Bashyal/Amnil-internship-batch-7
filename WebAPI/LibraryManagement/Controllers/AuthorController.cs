using LibraryManagement.DTO;
using LibraryManagement.Model;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

/// <summary>
/// Author API Controller demonstrating CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Create a new Author
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Author>> CreateUser(AuthorCreateDto authorModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdAuthor = await _authorService.CreateAuthorAsync(authorModel);
        return StatusCode(StatusCodes.Status201Created, createdAuthor);
    }

    [HttpGet("{bookId}")]
    public async Task<ActionResult<Author>> GetByBookId(int bookId)
    {
        var author = await _authorService.GetByBookIdAsync(bookId);
        if (author == null)
            return NotFound();
        return Ok(author);
    }

    [Route("/byBookCount")]
    [HttpGet]
    public async Task<ActionResult<GetAuthorWithBookCountDto>> GetAuthorBYBookCount()
    {
        var Authors = await _authorService.GetBookCountPerAuthorAsync();
        return Ok(Authors);
    }
}
