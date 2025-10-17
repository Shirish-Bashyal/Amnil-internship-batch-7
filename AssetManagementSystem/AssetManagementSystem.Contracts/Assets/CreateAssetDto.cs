using System.ComponentModel.DataAnnotations;
using System.Data;
using AssetManagementSystem.Shared.Constants;

namespace AssetManagementSystem.Contracts.Assets;

/// <summary>
///Dto to create a new asset
/// </summary>
public record CreateAssetDto
{
    [Required(ErrorMessage = "Asset name is required.")]
    [MaxLength(AssetConsts.Name.MaxLength, ErrorMessage = "Name cannot exceed {1} characters.")]
    [RegularExpression(
        @"^[a-zA-Z\s]+$",
        ErrorMessage = "Name must contain only letters and space."
    )]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = " Assest`s serial number is required.")]
    [MaxLength(
        AssetConsts.SerialNumber.MaxLength,
        ErrorMessage = "Serial number cannot exceed {1} characters."
    )]
    public string SerialNumber { get; set; } = string.Empty;

    [MaxLength(
        AssetConsts.Description.MaxLength,
        ErrorMessage = "Description cannot exceed {1} characters."
    )]
    public string? Description { get; set; }

    public DateTime? ReceivedDate { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }

    public Guid? DepartmentId { get; set; }
}
