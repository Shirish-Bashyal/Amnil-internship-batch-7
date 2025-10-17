using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace AssetManagementSystem.Entity.Entities;

public class BaseModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool IsActive { get; set; } = true;

    [MaxLength(EntityConstrains.BaseEntity.DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [MaxLength(EntityConstrains.BaseEntity.CreatedByMaxLength)]
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [MaxLength(EntityConstrains.BaseEntity.ModifiedByMaxLength)]
    public string? ModifiedBy { get; set; }
}
