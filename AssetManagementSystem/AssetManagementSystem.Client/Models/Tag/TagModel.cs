using System.ComponentModel.DataAnnotations;
using AssetManagementSystem.Shared.Constants;

namespace AssetManagementSystem.Client.Models.Tag;

/// <summary>
/// represents tag details to create a tag entity
/// </summary>
public class TagModel
{
    [Required(ErrorMessage = "Mac Address cannot be empty.")]
    [MaxLength(
        TagConsts.MacAddress.MaxLength,
        ErrorMessage = "Mac Address cannot be more than {0} characters"
    )]
    public string MacAddress { get; set; } = string.Empty;
}
