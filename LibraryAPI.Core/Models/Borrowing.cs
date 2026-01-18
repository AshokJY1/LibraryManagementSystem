namespace LibraryAPI.Core.Models;

public class Borrowing
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime BorrowedDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string Status { get; set; } = "Borrowed";

    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}