using LibraryAPI.Core.DTOs;

namespace LibraryAPI.Core.Interfaces;

public interface IBorrowingService
{
    Task<BorrowingResponseDto> BorrowBookAsync(int bookId, int userId);
    Task<BorrowingResponseDto> ReturnBookAsync(int borrowingId, int userId);
    Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingsAsync(int userId);
    Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingHistoryAsync(int userId);
}