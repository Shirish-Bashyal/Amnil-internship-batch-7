using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTO;

/// <summary>
/// Dto used to update existing book entity
/// </summary>
public class BookUpdateDto
{
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    public string ISBN { get; set; } = null!;

    public int PublishedYear { get; set; }

    public int AuthorId { get; set; }
}
