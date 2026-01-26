using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities.Application;

/// <summary>
/// Book entity representing a book in the system
/// </summary>
public class Book : DateAuditedEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public Guid AuthorId { get; set; }

    // Navigation property, virtual for Lazy Loading
    [ForeignKey("AuthorId")]
    public virtual Author Author { get; set; } = null!;

    //many to many relationship
    public virtual ICollection<BorrowerBook> BorrowerBooks { get; set; } = new List<BorrowerBook>();
}
