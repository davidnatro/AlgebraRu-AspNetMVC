using mednik.Data.Repositories.Groups;
using mednik.Data.Repositories.Subjects;
using mednik.Models;
using mednik.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class SubjectsController : Controller
{
    private readonly ISubjectsRepository _subjectsRepository;

    private readonly IGroupsRepository _groupsRepository;

    public SubjectsController(ISubjectsRepository subjectsRepository, IGroupsRepository groupsRepository)
    {
        _subjectsRepository = subjectsRepository;

        _groupsRepository = groupsRepository;
    }

    /// <summary>
    /// Страница для добавления предмета.
    /// </summary>
    /// <returns>Страница для добавления предмета.</returns>
    public IActionResult Add() => View();

    /// <summary>
    /// Добавляет предмет в базу данных.
    /// </summary>
    /// <param name="name">Имя предмета</param>
    /// <returns>Домашнюю страницу</returns>
    public async Task<IActionResult> AddSubjectToDB(string name)
    {
        var subject = new Subject()
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поле не должно оставаться пустым!");
            return View("Add", subject);
        }

        await _subjectsRepository.AddAsync(subject);
        
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Удаляет предмет из базы данных
    /// </summary>
    /// <param name="id">Id предмета</param>
    /// <returns>Домашнюю страницу</returns>
    public async Task<IActionResult> Delete(Guid id)
    {
        await _subjectsRepository.DeleteAsync(id);

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Страница со списком групп (выбранного предмета)
    /// </summary>
    /// <param name="id">Id предмета</param>
    /// <returns>Страница со списком групп (выбранного предмета)</returns>
    [AllowAnonymous]
    public async Task<IActionResult> Groups(Guid id)
    {
        var groups = await _groupsRepository.GetAllBySubjectIdAsync(id);

        var data = new SubjectIdAndGroups()
        {
            SubjectId = id,
            Groups = groups
        };

        return View(data);
    }
}