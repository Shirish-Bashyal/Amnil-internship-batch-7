using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;


namespace AssetManagementSystem.Shared.Dtos.Department;

public class DepartmentDto
{

    public Guid Id { get; set; }

    [Required]
    [MaxLength(EntityConstrains.Department.NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(EntityConstrains.BaseEntity.DescriptionMaxLength)]
    public string? Description { get; set; }

   
}
