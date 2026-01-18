using System.Linq;
using LibraryAPI.Core.DTOs;
using LibraryAPI.Core.Interfaces;
using LibraryAPI.Core.Models;

namespace LibraryAPI.Core.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToResponseDto).ToList();
    }

    public async Task<BookResponseDto?> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return book != null ? MapToResponseDto(book) : null;
    }

    public async Task<BookResponseDto> AddBookAsync(BookDto bookDto)
    {
        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            ISBN = bookDto.ISBN,
            PublishedYear = bookDto.PublishedYear,
            TotalCopies = bookDto.TotalCopies,
            AvailableCopies = bookDto.TotalCopies
        };

        var addedBook = await _bookRepository.AddAsync(book);
        return MapToResponseDto(addedBook);
    }

    public async Task<BookResponseDto?> UpdateBookAsync(int id, BookDto bookDto)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return null;

        var copiesDiff = bookDto.TotalCopies - book.TotalCopies;

        book.Title = bookDto.Title;
        book.Author = bookDto.Author;
        book.ISBN = bookDto.ISBN;
        book.PublishedYear = bookDto.PublishedYear;
        book.TotalCopies = bookDto.TotalCopies;
        book.AvailableCopies = Math.Max(0, book.AvailableCopies + copiesDiff);

        var updatedBook = await _bookRepository.UpdateAsync(book);
        return MapToResponseDto(updatedBook);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string query)
    {
        var books = await _bookRepository.SearchAsync(query);
        return books.Select(MapToResponseDto).ToList();
    }

    private static BookResponseDto MapToResponseDto(Book book)
    {
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            PublishedYear = book.PublishedYear,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            CreatedAt = book.CreatedAt
        };
    }
}

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Book>> SearchAsync(string query);
}