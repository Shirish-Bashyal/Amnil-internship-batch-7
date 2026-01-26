using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Client.Models.Assets;

public class AssignUserModel
{
    [Required(ErrorMessage = "UserId is required.")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "AssetId is required.")]
    public string AssetId { get; set; }

    //location for asset

    [Required(ErrorMessage = "BuildingId is required.")]
    public string BuildingId { get; set; }

    public string? Floor { get; set; }

    public string? Room { get; set; }
}
