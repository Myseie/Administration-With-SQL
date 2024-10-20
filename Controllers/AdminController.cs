using Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRolesViewModel = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRolesViewModel.Add(new UserRolesViewModel
            {
                User = user,
                Roles = roles
            });
        }

        return View(userRolesViewModel);
    }

    public IActionResult Roles()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(string userId, string role)
    {

        if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
        {
            TempData["Error"] = "User ID and Role cannot be empty.";    
            return RedirectToAction("Index"); 
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && await _roleManager.RoleExistsAsync(role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{role}' added to user {user.UserName}.";
            }
            else
            {
                TempData["Error"] = $"Failed to add role '{role}' to user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveRole(string userId, string role)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
        {
            return RedirectToAction("Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{role}' removed from user {user.UserName}.";
            }
            else
            {
                TempData["Error"] = $"Failed to remove role '{role}' from user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
        }

        return RedirectToAction("Index");
    }


}