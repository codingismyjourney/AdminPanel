using AdminPanelWebAPI.DTOs;
using AdminPanelWebApp.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelWebApp.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUsersAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            await _userService.RegisterAsync(registerDto);
            return RedirectToAction(nameof(Index));
        }
        return View(registerDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateUserDto updateUserDto)
    {
        if (ModelState.IsValid)
        {
            await _userService.UpdateUserAsync(id, updateUserDto);
            return RedirectToAction(nameof(Index));
        }
        return View(updateUserDto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
