using AttendanceManagementSystem.Contracts.Interfaces.Attendance;
using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.Attendance;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.API.Controllers;

/// <summary>
/// Controller representing endpoints for Attendance Record entity
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    /// <summary>
    ///Retrives all AttendanceRecord based on userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>

    [HttpGet("{userId}")]
    //[Authorize(Policy = "ApiKeyPolicy")]
    public async Task<ActionResult<ServiceResponseDto<IEnumerable<AttendanceDto>>>> GetByUserId(
        string userId
    )
    {
        if (!Guid.TryParse(userId, out Guid guidId))
        {
            return BadRequest();
        }

        var result = await _attendanceService.GetByUserIdAsync(guidId);
        return Ok(result);
    }

    /// <summary>
    ///Creates a Attendance Record with checkin time for User based on Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("check-in/{userId}")]
    public async Task<ActionResult<ServiceResponseDto<AttendanceDto>>> CheckIn(string userId)
    {
        if (!Guid.TryParse(userId, out Guid guidId))
        {
            return BadRequest();
        }
        var result = await _attendanceService.CheckInAsync(guidId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    /// <summary>
    ///Updates Attendance Record with checkout time for User based on Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPatch("check-out/{userId}")]
    public async Task<ActionResult<ServiceResponseDto<AttendanceDto>>> CheckOut(string userId)
    {
        if (!Guid.TryParse(userId, out Guid guidId))
        {
            return BadRequest();
        }
        var result = await _attendanceService.CheckOutAsync(guidId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }
}
