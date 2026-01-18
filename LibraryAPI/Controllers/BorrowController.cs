using System.Security.Claims;
using LibraryAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BorrowController : ControllerBase
{
    private readonly IBorrowingService _borrowingService;
    private readonly ILogger<BorrowController> _logger;

    public BorrowController(IBorrowingService borrowingService, ILogger<BorrowController> logger)
    {
        _borrowingService = borrowingService;
        _logger = logger;
    }

    [HttpPost("{bookId}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> BorrowBook(int bookId)
    {
        try
        {
            var userId = GetUserId();
            var borrowing = await _borrowingService.BorrowBookAsync(bookId, userId);
            return Ok(borrowing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to borrow book {BookId}", bookId);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("return/{borrowingId}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> ReturnBook(int borrowingId)
    {
        try
        {
            var userId = GetUserId();
            var borrowing = await _borrowingService.ReturnBookAsync(borrowingId, userId);
            return Ok(borrowing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to return book for borrowing {BorrowingId}", borrowingId);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my-books")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetMyBorrowedBooks()
    {
        var userId = GetUserId();
        var borrowings = await _borrowingService.GetUserBorrowingsAsync(userId);
        return Ok(borrowings);
    }

    [HttpGet("history")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetBorrowingHistory()
    {
        var userId = GetUserId();
        var history = await _borrowingService.GetUserBorrowingHistoryAsync(userId);
        return Ok(history);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }
}
