using Contract.Interfaces.Author;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApplication.Controllers;

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
    public async Task<ActionResult<ServiceResponseDto<Guid>>> Create(
        [FromBody] CreateAuthorDto authorModel
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdAuthor = await _authorService.CreateAsync(authorModel);
        return StatusCode(StatusCodes.Status201Created, createdAuthor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponseDto<Guid>>> Update(
        string id,
        [FromBody] UpdateAuthorDto authorModel
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        var result = await _authorService.UpdateAsync(guidId, authorModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result);
    }

    [HttpGet("all")]
    public async Task<ActionResult<ServiceResponseDto<IEnumerable<GetAuthorDto>>>> GetAll()
    {
        var result = await _authorService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<ActionResult<ServiceResponseDto<IEnumerable<GetAuthorDto>>>> GetByPage(
        int pageNumber,
        int pageSize
    )
    {
        var result = await _authorService.GetByPage(pageNumber, pageSize);
        if (result.Data?.Items == null)
            return NotFound(result);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponseDto<GetAuthorDto>>> GetById(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }
        var result = await _authorService.GetByIdAsync(guidId);
        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponseDto<Guid>>> Delete(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        var result = await _authorService.DeleteAsync(guidId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
}
