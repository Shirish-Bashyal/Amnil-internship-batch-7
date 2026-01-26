using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Contracts.Assets;

/// <summary>
/// represents input required to assign a asset to a user
/// </summary>
public record AssignUserDto
{
    [Required(ErrorMessage = "UserId is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "AssetId is required.")]
    public Guid AssetId { get; set; }

    //location for asset

    [Required(ErrorMessage = "BuildingId is required.")]
    public Guid BuildingId { get; set; }

    public string? Floor { get; set; }

    public string? Room { get; set; }
}
