using LibraryManagement.DTO;
using LibraryManagement.Model;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LibraryManagement.Controllers;

/// <summary>
///  Borrower API Controller demonstrating CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BorrowerController : ControllerBase
{
    private readonly IBorrowService _borrowerService;

    public BorrowerController(IBorrowService borrowerService)
    {
        _borrowerService = borrowerService;
    }

    /// <summary>
    /// Get all borrowers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers()
    {
        var borrowers = await _borrowerService.GetAllBorrowersAsync();
        return Ok(borrowers);
    }

    /// <summary>
    /// Deletes a borrower
    /// </summary>
    /// <param name="id"> represents the id of the borrower to be deleted </param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBorrower(int id)
    {
        var isDeleted = await _borrowerService.DeleteBorrowerAsync(id);
        if (isDeleted)
        {
            return NoContent();
        }
        return NotFound();
    }

    [Route("/groupedByCount")]
    [HttpGet]
    public async Task<
        ActionResult<List<GetBorrowersGroupByBookCountDto>>
    > GetBorrowersGroupByBookCount()
    {
        var borrowersGroupedByBookCount = await _borrowerService.GetBorrowersGroupByBookCount();
        return Ok(borrowersGroupedByBookCount);
    }
}
