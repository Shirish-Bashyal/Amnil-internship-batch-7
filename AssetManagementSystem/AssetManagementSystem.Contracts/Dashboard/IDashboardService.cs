using AssetManagementSystem.Shared.Dtos;

namespace AssetManagementSystem.Contracts.Dashboard;

/// <summary>
/// defines operations for dashboard
/// </summary>
public interface IDashboardService
{
    Task<ResponseDto> GetAsync();
}
