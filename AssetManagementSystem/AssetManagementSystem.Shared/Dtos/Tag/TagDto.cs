using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;


namespace AssetManagementSystem.Shared.Dtos.Tag;

public class TagDto
{

    public Guid TagId { get; set; }


    [Required]
    [MaxLength(EntityConstrains.Tag.MacAddressMaxLength)]
    public string MacAddress { get; set; } = string.Empty;

    // Optional: assign to an asset
    public Guid AssetId { get; set; }

 
    public bool IsActive { get; set; }
    // Optional description or metadata
    public string? Description { get; set; }

}
