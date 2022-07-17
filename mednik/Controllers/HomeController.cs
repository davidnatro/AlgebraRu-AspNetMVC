using mednik.Data;
using mednik.Data.Base;
using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;


public class HomeController : Controller
{
    // GET
    public async Task<IActionResult> Index() => View();
}