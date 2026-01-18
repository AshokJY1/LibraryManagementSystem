using System.Linq;
using LibraryAPI.Core.DTOs;
using LibraryAPI.Core.Interfaces;
using LibraryAPI.Core.Models;

namespace LibraryAPI.Core.Services;

public class BorrowingService : IBorrowingService
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IBookRepository _bookRepository;

    public BorrowingService(IBorrowingRepository borrowingRepository, IBookRepository bookRepository)
    {
        _borrowingRepository = borrowingRepository;
        _bookRepository = bookRepository;
    }

    public async Task<BorrowingResponseDto> BorrowBookAsync(int bookId, int userId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book == null)
            throw new Exception("Book not found");

        if (book.AvailableCopies <= 0)
            throw new Exception("No copies available");

        var borrowing = new Borrowing
        {
            BookId = bookId,
            UserId = userId,
            BorrowedDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            Status = "Borrowed"
        };

        book.AvailableCopies--;
        await _bookRepository.UpdateAsync(book);

        var createdBorrowing = await _borrowingRepository.AddAsync(borrowing);
        return MapToResponseDto(createdBorrowing);
    }

    public async Task<BorrowingResponseDto> ReturnBookAsync(int borrowingId, int userId)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId);

        if (borrowing == null)
            throw new Exception("Borrowing record not found");

        if (borrowing.UserId != userId)
            throw new Exception("Unauthorized to return this book");

        if (borrowing.Status == "Returned")
            throw new Exception("Book already returned");

        borrowing.ReturnedDate = DateTime.UtcNow;
        borrowing.Status = "Returned";

        var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
        if (book != null)
        {
            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book);
        }

        var updatedBorrowing = await _borrowingRepository.UpdateAsync(borrowing);
        return MapToResponseDto(updatedBorrowing);
    }

    public async Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingsAsync(int userId)
    {
        var borrowings = await _borrowingRepository.GetActiveBorrowingsByUserAsync(userId);
        return borrowings.Select(MapToResponseDto).ToList();
    }

    public async Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingHistoryAsync(int userId)
    {
        var borrowings = await _borrowingRepository.GetBorrowingHistoryByUserAsync(userId);
        return borrowings.Select(MapToResponseDto).ToList();
    }

    private static BorrowingResponseDto MapToResponseDto(Borrowing borrowing)
    {
        return new BorrowingResponseDto
        {
            Id = borrowing.Id,
            BookId = borrowing.BookId,
            BookTitle = borrowing.Book?.Title ?? "",
            BookAuthor = borrowing.Book?.Author ?? "",
            BorrowedDate = borrowing.BorrowedDate,
            DueDate = borrowing.DueDate,
            ReturnedDate = borrowing.ReturnedDate,
            Status = borrowing.Status
        };
    }
}

public interface IBorrowingRepository
{
    Task<Borrowing?> GetByIdAsync(int id);
    Task<Borrowing> AddAsync(Borrowing borrowing);
    Task<Borrowing> UpdateAsync(Borrowing borrowing);
    Task<IEnumerable<Borrowing>> GetActiveBorrowingsByUserAsync(int userId);
    Task<IEnumerable<Borrowing>> GetBorrowingHistoryByUserAsync(int userId);
}