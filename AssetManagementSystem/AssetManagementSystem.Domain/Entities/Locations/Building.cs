using AssetManagementSystem.Domain.Base;

namespace AssetManagementSystem.Domain.Entities.Locations;

/// <summary>
/// Represents a building entity
/// </summary>
public class Building : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
}
