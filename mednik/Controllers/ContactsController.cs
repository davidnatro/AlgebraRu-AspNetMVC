using mednik.Data;
using mednik.Data.Repositories.Contacts;
using Microsoft.AspNetCore.Authorization;
using io = System.IO;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class ContactsController : Controller
{
    private readonly IContactsRepository _contactsRepository;

    public ContactsController(IContactsRepository contactsRepository)
    {
        _contactsRepository = contactsRepository;
    }

    [Authorize]
    public async Task<IActionResult> Edit(string id, string? name, string? telegram)
    {
        await _contactsRepository.ChangeData(id, name, telegram);

        return RedirectToAction("Index");
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var users = await _contactsRepository.GetAllAsync();

        return View(users);
    }
}