using System.ComponentModel.DataAnnotations;
using AssetManagementSystem.Shared.Constants;

namespace AssetManagementSystem.Contracts.Tags;

/// <summary>
/// dto to create a Tag entity
/// </summary>
public record CreateTagDto
{
    [Required(ErrorMessage = "Mac Address cannot be empty.")]
    [MaxLength(
        TagConsts.MacAddress.MaxLength,
        ErrorMessage = "Mac Address cannot be more than {0} characters"
    )]
    public string MacAddress { get; set; } = string.Empty;
}
