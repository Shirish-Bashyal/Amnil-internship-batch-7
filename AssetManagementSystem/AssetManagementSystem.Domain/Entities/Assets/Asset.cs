using System.ComponentModel.DataAnnotations.Schema;
using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Tags;

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

    public DateTime? ReceivedDate { get; set; } // date in which the department received the asset
    public bool IsActive { get; set; }

    //navigation properties
    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;

    public Guid? DepartmentId { get; set; } //nullable since asset can also  not be assigned to any department

    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; } = null!;

    public virtual Tag? Tag { get; set; } //Nullable if the asset does not have a tag.
}
