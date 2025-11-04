using System.Runtime.CompilerServices;
using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Domain.Entities.Assets;

namespace AssetManagementSystem.Application.Assets;

/// <summary>
///query extensions for Asset entity
/// </summary>
public static class AssetExtensions
{
    /// <summary>
    /// Searches assets by normalized name or serial number
    /// </summary>
    public static IQueryable<Asset> Search(this IQueryable<Asset> query, string? searchTerm)
    {
        searchTerm = searchTerm?.Trim().ToUpper();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }

        return query.Where(x =>
            x.NormalizedName.Contains(searchTerm) || x.NormalizedSerialNumber.Contains(searchTerm)
        );
    }

    /// <summary>
    /// Applies sorting
    /// </summary>
    public static IQueryable<Asset> Sort(this IQueryable<Asset> query, string? sortOrder)
    {
        sortOrder = sortOrder?.Trim();

        if (string.IsNullOrWhiteSpace(sortOrder))
        {
            return query;
        }

        query = sortOrder switch
        {
            "name_desc" => query.OrderByDescending(u => u.NormalizedName),
            "name_asc" => query.OrderBy(u => u.NormalizedName),

            "date_asc" => query.OrderBy(u => u.ReceivedDate),
            "date_desc" => query.OrderByDescending(u => u.ReceivedDate),

            "serialNumber_desc" => query.OrderByDescending(u => u.NormalizedSerialNumber),
            "serialNumber_asc" => query.OrderBy(u => u.NormalizedSerialNumber),

            "description_desc" => query.OrderByDescending(u => u.Description),
            "description_asc" => query.OrderBy(u => u.Description),

            "category_desc" => query.OrderByDescending(u => u.Category.Name),
            "category_asc" => query.OrderBy(u => u.Category.Name),

            "department_desc" => query.OrderByDescending(u => u.Department.Name),
            "department_asc" => query.OrderBy(u => u.Department.Name),

            _ => query.OrderByDescending(u => u.ReceivedDate)
        };

        return query;
    }

    /// <summary>
    /// Filters assets by category and active status
    /// </summary>
    public static IQueryable<Asset> Filter(
        this IQueryable<Asset> query,
        Guid? categoryId,
        bool? isActive
    )
    {
        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }
        return query;
    }

    /// <summary>
    /// maps Asset entity to AssetDto
    /// </summary>
    public static IQueryable<AssetDto> ToDto(this IQueryable<Asset> query)
    {
        return query.Select(x => new AssetDto
        {
            Id = x.Id,
            Name = x.Name,
            SerialNumber = x.SerialNumber,
            DepartmentId = x.DepartmentId,
            CategoryId = x.CategoryId,
            Description = x.Description,
            IsActive = x.IsActive,
            ReceivedDate = x.ReceivedDate,
            Category = x.Category.Name,
            Department = x.Department.Name,
            TagMacAddress = x.Tag == null ? null : x.Tag.MacAddress
        });
    }
}
