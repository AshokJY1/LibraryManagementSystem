using LibraryAPI.Core.DTOs;
using LibraryAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for user {Username}", registerDto.Username);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var (token, username, role) = await _authService.LoginAsync(loginDto);
            return Ok(new { token, username, role });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user {Username}", loginDto.Username);
            return Unauthorized(new { message = ex.Message });
        }
    }
}