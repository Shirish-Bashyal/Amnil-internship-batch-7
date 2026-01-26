namespace LibraryManagement.Model;

/// <summary>
/// Borrower entity representing a user who borrows a book
/// </summary>
public class Borrower
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Navigation property, virtual for Lazy Loading
    //many to many relationship
    public virtual ICollection<BorrowerBook> BorrowerBooks { get; set; } = new List<BorrowerBook>();
}
