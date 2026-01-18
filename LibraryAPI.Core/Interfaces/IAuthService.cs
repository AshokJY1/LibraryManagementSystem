using LibraryAPI.Core.DTOs;

namespace LibraryAPI.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto registerDto);
    Task<(string token, string username, string role)> LoginAsync(LoginDto loginDto);
}