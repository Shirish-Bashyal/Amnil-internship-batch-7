using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Contracts.Assets;

public record AssignTagDto
{
    [Required(ErrorMessage = "TagId is required.")]
    public Guid TagId { get; set; }

    [Required(ErrorMessage = "AsetId is required.")]
    public Guid AssetId { get; set; }
}
