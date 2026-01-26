using Domain.Entities.Base;

namespace Domain.Entities.Application;

/// <summary>
/// Author entity representing book authors
/// </summary>
public class Author : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    // Navigation property, virtual for Lazy Loading

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
