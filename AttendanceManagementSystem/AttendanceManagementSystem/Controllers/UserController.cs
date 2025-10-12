using AttendanceManagementSystem.Contracts.Interfaces.User;
using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.User;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.API.Controllers;

/// <summary>
/// User Api Controller demonstrating CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userModel"> user details to be created</param>
    /// <returns> newly created users Id</returns>
    [HttpPost]
    // [Authorize]
    public async Task<ActionResult<ServiceResponseDto<Guid>>> Create([FromBody] UserDto userModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.CreateAsync(userModel);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// updates an existing user.
    /// </summary>
    /// <param name="id"> The Id of the user to update</param>
    /// <param name="userModel">The details of the user to update </param>
    /// <returns> return success status </returns>

    [HttpPut("{id}")]
    //[Authorize]
    public async Task<ActionResult<ServiceResponseDto<bool>>> Update(
        string id,
        [FromBody] UserDto userModel
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        var result = await _userService.UpdateAsync(guidId, userModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result);
    }

    /// <summary>
    /// Retrives users with pagination
    /// </summary>
    /// <returns> a list of users</returns>
    //[Authorize(Policy = "ApiKeyPolicy")]
    [HttpGet("List")]
    public async Task<ActionResult<ServiceResponseDto<PagedResponseDto<List<GetUserDto>>>>> GetList(
        [FromQuery] string? SearchTerm,
        [FromQuery] PagedRequestDto input,
        [FromQuery] string? sortOrder
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var result = await _userService.GetListAsync(input, SearchTerm, sortOrder);
        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    /// Retrives all users
    /// </summary>
    /// <returns> All users</returns>
    [Authorize(Policy = "ApiKeyPolicy")]
    [HttpGet]
    public async Task<ActionResult<ServiceResponseDto<IEnumerable<GetUserDto>>>> GetAll()
    {
        var result = await _userService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    /// <summary>
    /// Retrives a user
    /// </summary>
    /// <param name="id"> The Id of the user to retrive</param>
    /// <returns> a user details</returns>
    [HttpGet("{id}")]
    [Authorize(Policy = "ApiKeyPolicy")]
    public async Task<ActionResult<ServiceResponseDto<GetUserDto>>> GetById(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }
        var result = await _userService.GetByIdAsync(guidId);
        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    /// <summary>
    /// Deletes a user by their Id
    /// </summary>
    /// <param name="id"> The Id of the user to delete </param>
    /// <returns> returns success status </returns>
    [HttpDelete("{id}")]
    //[Authorize]
    public async Task<ActionResult<ServiceResponseDto<bool>>> Delete(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        var result = await _userService.DeleteAsync(guidId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
}
