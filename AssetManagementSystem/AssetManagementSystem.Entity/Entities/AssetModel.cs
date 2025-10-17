using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AssetManagementSystem.Entity.Entities;

public class AssetModel:BaseModel
{
    
    [Required]
    [MaxLength(EntityConstrains.Asset.AssetNameMaxLength)]
    public string AssetName { get; set; } = string.Empty;

    [Required]
    [MaxLength(EntityConstrains.Asset.SerialNumberMaxLength)]
    public string SerialNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(EntityConstrains.Asset.AssetCategoryMaxLength)]
    public string AssetCategory { get; set; } = string.Empty;

    [Required]
    public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsActivated { get; set; } = true;

    [Required]
    public Guid DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public DepartmentModel? Department { get; set; }

    // Navigation property for one-to-one
    public TagModel? Tag { get; set; }
}

