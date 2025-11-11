using AssetManagementSystem.Contracts.Dashboard;
using AssetManagementSystem.Contracts.Repositories;
using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Tags;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagementSystem.Application.Dashboard;

/// <summary>
/// Implementation of operations for dashboard
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly ILogger<DashboardService> _logger;
    private readonly IGenericRepository<Asset> _assetRepo;
    private readonly IGenericRepository<Tag> _tagRepo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<Department> _departmentRepo;

    public DashboardService(
        ILogger<DashboardService> logger,
        IGenericRepository<Asset> assetRepo,
        IGenericRepository<Tag> tagRepo,
        IGenericRepository<Category> categoryRepo,
        IGenericRepository<Department> departmentRepo
    )
    {
        _logger = logger;
        _assetRepo = assetRepo;
        _tagRepo = tagRepo;
        _categoryRepo = categoryRepo;
        _departmentRepo = departmentRepo;
    }

    /// <summary>
    /// Gets all details required for dashboard
    /// </summary>
    public async Task<ResponseDto> GetAsync()
    {
        _logger.LogInformation(" ::DashboardService:: - GetAsync - ::Started::");
        try
        {
            var totalAssets = await _assetRepo.GetQueryable().CountAsync();
            var totalActiveAssets = await _assetRepo
                .GetQueryable()
                .Where(x => x.IsActive)
                .CountAsync();

            var totalTags = await _tagRepo.GetQueryable().CountAsync();
            var totalActiveTags = await _tagRepo.GetQueryable().Where(x => x.IsActive).CountAsync();

            var totalCategories = await _categoryRepo.GetQueryable().CountAsync();

            var totalDepartments = await _departmentRepo.GetQueryable().CountAsync();

            var dashboardDto = new DashboardDto
            {
                ActiveAssets = totalActiveAssets,
                Tags = totalTags,
                ActiveTags = totalActiveTags,
                Departments = totalDepartments,
                Categories = totalCategories,
                Assets = totalAssets
            };

            return ResponseDto<DashboardDto>.Success(dashboardDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " ::DashboardService:: - GetAsync - ::Exception:: ");

            return ResponseDto.InternalServerError("An Unexpected error occured.");
        }
        finally
        {
            _logger.LogInformation("::DashboardService:: - GetAsync - ::Ended::");
        }
    }
}
