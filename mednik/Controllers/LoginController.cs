using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}