using AdminPanelWebAPI.Data;
using AdminPanelWebAPI.DTOs;
using AdminPanelWebAPI.Entities;
using AdminPanelWebAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdminPanelWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;
    private readonly SignInManager<AppUser> _signInManager;

    public UserController(UserManager<AppUser> userManager,
                          ITokenService tokenService,
                          DataContext context,
                          SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(x => new UserDto
        {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Gender = x.Gender
        }).ToList();
    }

    [HttpGet("getCurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID from the claims
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender
        };

        return Ok(userDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender
        };

        return Ok(userDto);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
        {
            return BadRequest("Email address already exists");
        }

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Gender = registerDto.Gender
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Customer");

        var userResponseDto = new UserResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };

        return Ok(userResponseDto);
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDto>> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized("User is not authorized");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("User is not authorized");

        var userResponseDto = new UserResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };

        return Ok(userResponseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.Gender = updateUserDto.Gender;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var userResponseDto = new UserResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };

        return Ok(userResponseDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    [HttpGet("emailExists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }
}
