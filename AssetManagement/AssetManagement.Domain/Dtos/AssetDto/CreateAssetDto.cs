using AssetManagement.Shared.Constant;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Dtos.AssetDto;

public class CreateAssetDto
{
    [Required(ErrorMessage = "Asset name is required.")]
    [StringLength(Constraints.Name.MaxLength, ErrorMessage = "Name can't exceed 100 characters.")]
    public string Name { get; set; }= string.Empty;

    [Required(ErrorMessage = "Asset SerialNumber is required.")]
    [StringLength(Constraints.SerialNumber.MaxLength, ErrorMessage = "SerialNumber can't exceed 100 characters.")]
    public string SerialNumber { get; set; }=string.Empty;
    public decimal Cost { get; set; }

    [Required(ErrorMessage = "Asset SerialNumber is required.")]
    public string MacAddress { get; set; }=string.Empty ;
    public string? CategoryName { get; set; }
}
