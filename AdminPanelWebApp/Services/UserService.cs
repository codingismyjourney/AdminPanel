using AdminPanelWebAPI.DTOs;
using AdminPanelWebApp.IServices;
using System.Net.Http.Headers;

namespace AdminPanelWebApp.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("AdminPanelAPI");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("user");
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
        return users;
    }

    public async Task<UserDto> GetCurrentUserAsync()
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("user/getCurrentUser");
        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user;
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"user/{id}");
        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user;
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("user/register", registerDto);
        response.EnsureSuccessStatusCode();

        var userResponseDto = await response.Content.ReadFromJsonAsync<UserResponseDto>();

        _httpContextAccessor.HttpContext.Session.SetString("Token", userResponseDto.Token);

        return userResponseDto;
    }

    public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("user/login", loginDto);
        response.EnsureSuccessStatusCode();

        var userResponseDto = await response.Content.ReadFromJsonAsync<UserResponseDto>();

        _httpContextAccessor.HttpContext.Session.SetString("Token", userResponseDto.Token);

        return userResponseDto;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync($"user/{id}", updateUserDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(int id)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"user/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        var response = await _httpClient.GetAsync($"user/emailExists?email={email}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<bool>();
    }
}
