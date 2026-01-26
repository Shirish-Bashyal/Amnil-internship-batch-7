using LibraryManagement.Model;

namespace LibraryManagement.DTO;

public class GetBorrowersGroupByBookCountDto
{
    public int BorrowedBookCount { get; set; }

    public List<Borrower> Borrowers { get; set; } = new List<Borrower>();
}
