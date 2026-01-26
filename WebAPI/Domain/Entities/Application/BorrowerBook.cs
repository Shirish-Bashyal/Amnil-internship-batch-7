using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities.Application;

/// <summary>
/// Represents join table for Borrower and Books entity
/// </summary>
public class BorrowerBook : Entity<Guid>
{
    public Guid BorrowerId { get; set; }
    public Guid BookId { get; set; }

    public DateTime BorrowedDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    //navigation properties

    [ForeignKey("BorrowerId")]
    public virtual Borrower Borrower { get; set; } = null!;

    [ForeignKey("BookId")]
    public virtual Book Book { get; set; } = null!;
}
