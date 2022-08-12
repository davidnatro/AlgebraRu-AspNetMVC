using mednik.Data.Repositories.Groups;
using mednik.Data.Repositories.Posts;
using mednik.Data.Repositories.Subjects;
using mednik.Models;
using mednik.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class GroupsController : Controller
{
    private readonly ISubjectsRepository _subjectsRepository;

    private readonly IGroupsRepository _groupsRepository;

    private readonly IPostsRepository _postsRepository;

    public GroupsController(ISubjectsRepository subjectsRepository, IGroupsRepository groupsRepository,
        IPostsRepository postsRepository)
    {
        _subjectsRepository = subjectsRepository;

        _groupsRepository = groupsRepository;

        _postsRepository = postsRepository;
    }

    public IActionResult AddGroup(Guid id)
    {
        SubjectIdAndGroupName subjectIdAndGroupName = new SubjectIdAndGroupName()
        {
            SubjectId = id
        };

        return View(subjectIdAndGroupName);
    }

    public IActionResult AddPost(Guid groupId)
    {
        GroupPostDTO postDto = new GroupPostDTO()
        {
            GroupdId = groupId
        };

        return View(postDto);
    }

    public async Task<IActionResult> SaveFileToGroup(GroupPostDTO postDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поля \"Название\" и \"Файл\" не должны быть пустыми!");
            return View("AddPost", postDto);
        }

        await _postsRepository.UploadFile(postDto.Name, postDto.Description, postDto.FileData, postDto.GroupdId);

        return Redirect("/Home");
    }

    [AllowAnonymous]
    public async Task<IActionResult> GroupFiles(Guid id)
    {
        GroupIdAndPosts idAndPosts = new GroupIdAndPosts()
        {
            GroupId = id,
            Posts = await _postsRepository.GetAllByGroupIdAsync(id)
        };

        return View(idAndPosts);
    }

    public async Task<IActionResult> AddGroupToSubject(SubjectIdAndGroupName model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поле не может оставаться пустым!");
            return View("AddGroup", model);
        }

        Group group = new Group()
        {
            Id = Guid.NewGuid(),
            Name = model.GroupName,
            SubjectId = model.SubjectId,
            Subject = await _subjectsRepository.GetByIdAsync(model.SubjectId)
        };

        await _groupsRepository.AddAsync(group);

        return Redirect($"/Home/Index");
    }

    public async Task<IActionResult> DeleteGroupFromSubject(Guid id)
    {
        await _groupsRepository.DeleteByIdAsync(id);

        return Redirect($"/Home/Index");
    }
}