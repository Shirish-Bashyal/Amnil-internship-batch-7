using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Assets;

namespace AssetManagementSystem.Domain.Entities.Categories;

/// <summary>
///represents categories of assets
/// </summary>
public class Category : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
