using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryManagement.Model;

/// <summary>
/// Represents join table for Borrower and Books entity
/// </summary>
public class BorrowerBook
{
    public int Id { get; set; }

    public int BorrowerId { get; set; }
    public int BookId { get; set; }

    //navigation properties
    [JsonIgnore]
    [ForeignKey("BorrowerId")]
    public virtual Borrower Borrower { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("BookId")]
    public virtual Book Book { get; set; } = null!;
}
