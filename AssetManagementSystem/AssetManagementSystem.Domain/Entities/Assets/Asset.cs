using System.ComponentModel.DataAnnotations.Schema;
using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Locations;
using AssetManagementSystem.Domain.Entities.Tags;
using AssetManagementSystem.Domain.Entities.Users;

namespace AssetManagementSystem.Domain.Entities.Assets;

/// <summary>
///Represent a asset in the application
/// </summary>
public class Asset : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string NormalizedName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string NormalizedSerialNumber { get; set; } = string.Empty;

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public DateTime? ReceivedDate { get; set; } // date in which the asset was received
    public bool IsActive { get; set; }

    //navigation properties

    public Guid? UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;

    public Guid? DepartmentId { get; set; } //nullable since asset can also  not be assigned to any department

    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public Guid? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location Location { get; set; } = null!;
}
