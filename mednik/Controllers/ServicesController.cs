using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class ServicesController : Controller
{
    private readonly IServicesRepository _servicesRepository;

    public ServicesController(IServicesRepository servicesRepository)
        => _servicesRepository = servicesRepository;

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> AddService(string name, string link)
    {
        var service = new Services() {Id = Guid.NewGuid(), Name = name, Link = link};

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, "Поля не должны оставаться пустыми!");
            return View("Index", service);
        }

        await _servicesRepository.AddAsync(service);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _servicesRepository.DeleteAsync(new Guid(id));

        return RedirectToAction("Index", "Home");
    }
}