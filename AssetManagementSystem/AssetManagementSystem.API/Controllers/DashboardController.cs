using AssetManagementSystem.Contracts.Dashboard;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

/// <summary>
/// Dashboard Controller exposing endpoints
/// </summary>

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// retrives all required  dashboard details
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ResponseDto>> Get()
    {
        var result = await _dashboardService.GetAsync();

        return StatusCode(result.StatusCode, result);
    }
}
