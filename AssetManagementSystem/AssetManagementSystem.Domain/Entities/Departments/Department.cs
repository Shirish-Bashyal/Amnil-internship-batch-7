using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Assets;

namespace AssetManagementSystem.Domain.Entities.Departments;

/// <summary>
///represents departments that owns assets
/// </summary>
public class Department : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
