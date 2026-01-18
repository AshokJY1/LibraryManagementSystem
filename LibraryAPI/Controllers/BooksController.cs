using LibraryAPI.Core.DTOs;
using LibraryAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound(new { message = "Book not found" });

        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Add([FromBody] BookDto bookDto)
    {
        try
        {
            var book = await _bookService.AddBookAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add book {Title}", bookDto.Title);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Update(int id, [FromBody] BookDto bookDto)
    {
        try
        {
            var book = await _bookService.UpdateBookAsync(id, bookDto);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update book {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        if (!result)
            return NotFound(new { message = "Book not found" });

        return Ok(new { message = "Book deleted successfully" });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Search query is required" });

        var books = await _bookService.SearchBooksAsync(query);
        return Ok(books);
    }
}