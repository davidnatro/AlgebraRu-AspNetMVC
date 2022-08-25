using mednik.Models;
using mednik.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class LoginController : Controller
{
    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
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
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поля не должны оставаться пустыми!");
            return View("Index", details);
        }
        
        var user = await _userManager.FindByEmailAsync(details.Email);
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

        ModelState.AddModelError(string.Empty, "Неверный логин или пароль!");

        // return RedirectToAction("Index", "Login", details);
        return View("Index", details);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}