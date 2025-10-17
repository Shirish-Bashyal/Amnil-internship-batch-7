using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;


namespace AssetManagementSystem.Entity.Entities;

public class DepartmentModel:BaseModel
{
    [Required]
    [MaxLength(EntityConstrains.Department.NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<AssetModel>? Assets { get; set; }
}
