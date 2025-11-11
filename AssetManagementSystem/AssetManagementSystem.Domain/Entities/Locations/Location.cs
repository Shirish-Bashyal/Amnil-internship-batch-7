using System.ComponentModel.DataAnnotations.Schema;
using AssetManagementSystem.Domain.Base;

namespace AssetManagementSystem.Domain.Entities.Locations;

/// <summary>
///Represents the location of asset
/// </summary>
public class Location : Entity<Guid>
{
    public string? Floor { get; set; }

    public string? Room { get; set; }

    public Guid BuildingId { get; set; }

    public virtual Building Building { get; set; } = null!;
}
