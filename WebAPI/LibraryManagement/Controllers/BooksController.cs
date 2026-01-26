using LibraryManagement.DTO;
using LibraryManagement.Model;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace LibraryManagement.Controllers;

/// <summary>
///  Book API Controller demonstrating CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// update an existing book based on id
    /// </summary>
    /// <param name="id"> represents the id of the book to update</param>
    /// <param name="bookModel"> represents the content to update</param>
    /// <returns></returns>

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> UpdateUser(int id, BookUpdateDto bookModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedBook = await _bookService.UpdateBookAsync(id, bookModel);
        if (updatedBook == null)
            return NotFound();

        return Ok(updatedBook);
    }

    [Route("byEagerLoading")]
    [HttpGet]
    public async Task<ActionResult<Book>> GetByEagerLoading()
    {
        var Books = await _bookService.GetAllWithAuthorByEager();
        return Ok(Books);
    }

    [Route("byLazyLoading")]
    [HttpGet]
    public async Task<ActionResult<Book>> GetByLazyLoading()
    {
        var Books = await _bookService.GetAllWithAuthorByLazy();
        return Ok(Books);
    }

    [HttpGet("{borrowerId}")]
    public async Task<ActionResult<Book>> GetBorrowedBooksByBorrowerId(int borrowerId)
    {
        var books = await _bookService.GetBorrowedBooksByBorrowerId(borrowerId);

        return Ok(books);
    }

    [Route("/topThree")]
    [HttpGet]
    public async Task<ActionResult<Book>> GetTopThreeBorrowedBooks()
    {
        var topBooks = await _bookService.GetTopThreeBorrowedBooks();
        return Ok(topBooks);
    }
}
