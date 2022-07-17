using mednik.Data;
using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class ServicesController : Controller
{
    private readonly IServicesRepository _repository;

    public ServicesController(AppDbContext repository)
    {
        _repository = new ServicesRepository(repository);
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> AddService(string name, string link)
    {
        Services service = new Services() {Name = name, Link = link};

        await _repository.AddAsync(service);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _repository.DeleteAsync(new Guid(id));
        
        return RedirectToAction("Index", "Home");
    }
}