using LibraryAPI.Core.DTOs;

namespace LibraryAPI.Core.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto?> GetBookByIdAsync(int id);
    Task<BookResponseDto> AddBookAsync(BookDto bookDto);
    Task<BookResponseDto?> UpdateBookAsync(int id, BookDto bookDto);
    Task<bool> DeleteBookAsync(int id);
    Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string query);
}