using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class ServicesController : Controller
{
    private readonly IServicesRepository _servicesRepository;

    public ServicesController(IServicesRepository servicesRepository)
        => _servicesRepository = servicesRepository;

    public IActionResult Index() => View();

    public IActionResult AddService(string? returnUrl)
    {
        ViewBag.returnUrl = returnUrl;
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddService(string name, string link, string? returnUrl)
    {
        var service = new Services() {Id = Guid.NewGuid(), Name = name, Link = link};

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поля не должны оставаться пустыми!");
            return View("Index", service);
        }

        await _servicesRepository.AddAsync(service);

        return Redirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, string? returnUrl)
    {
        await _servicesRepository.DeleteAsync(id);

        return Redirect(returnUrl ?? "/");
    }
}