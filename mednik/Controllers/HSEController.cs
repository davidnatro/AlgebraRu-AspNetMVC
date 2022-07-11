using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class HSEController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}