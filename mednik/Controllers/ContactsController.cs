using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class ContactsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}