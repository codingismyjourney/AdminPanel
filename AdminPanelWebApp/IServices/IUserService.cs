using AdminPanelWebAPI.DTOs;

namespace AdminPanelWebApp.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetCurrentUserAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<UserResponseDto> LoginAsync(LoginDto loginDto);
    Task UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task DeleteUserAsync(int id);
    Task<bool> CheckEmailExistsAsync(string email);
}
