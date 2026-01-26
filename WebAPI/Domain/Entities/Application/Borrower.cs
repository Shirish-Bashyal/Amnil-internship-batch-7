using Domain.Entities.Base;

namespace Domain.Entities.Application;

/// <summary>
/// Borrower entity representing a user who borrows a book
/// </summary>
public class Borrower : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Navigation property, virtual for Lazy Loading
    //many to many relationship
    public virtual ICollection<BorrowerBook> BorrowerBooks { get; set; } = new List<BorrowerBook>();
}
