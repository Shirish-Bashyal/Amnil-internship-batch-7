using AssetManagement.Shared.Constant;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AssetManagement.Domain.Dtos.AssetDto;

public class UpdateAssetDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Asset name is required.")]
    [StringLength(Constraints.Name.MaxLength, ErrorMessage = "Name can't exceed 100 characters.")]
    public string Name { get; set; }=string.Empty;
    [Required(ErrorMessage = "Cost is required.")]
    public decimal Cost { get; set; }
    public Guid? UserId { get; set; }

    [StringLength(Constraints.Name.MaxLength, ErrorMessage = "UserName can't exceed 100 characters.")]
    public string? UserName { get; set; }
    public string? DepartmentName { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool Status { get; set; }
}
