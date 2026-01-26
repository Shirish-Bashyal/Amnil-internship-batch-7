using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTO;

/// <summary>
/// Dto used for creating a new author
/// </summary>
public class AuthorCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Country { get; set; } = null!;
}
