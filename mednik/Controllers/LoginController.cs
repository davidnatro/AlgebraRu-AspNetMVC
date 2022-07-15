using System.ComponentModel.DataAnnotations;
using mednik.Models;
using mednik.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class LoginController : Controller
{
    private UserManager<User> _userManager;

    private SignInManager<User> _signInManager;

    public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.returnUrl = returnUrl;
        return View("Index");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel details, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByEmailAsync(details.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    user, details.Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect(returnUrl ?? "/");
                }
            }

            ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or password");
        }

        return RedirectToAction("Index", "Login", details);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}