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

    public GroupsController(ISubjectsRepository subjectsRepository, IGroupsRepository groupsRepository, IPostsRepository postsRepository)
    {
        _subjectsRepository = subjectsRepository;
        
        _groupsRepository = groupsRepository;
        
        _postsRepository = postsRepository;
    }
    
    public IActionResult AddGroup(Guid id) => View(id);

    public IActionResult AddPost(Guid groupId) => View(groupId);

    [AllowAnonymous]
    public async Task<IActionResult> GroupFiles(Guid id)
    {
        GroupIdAndPosts idAndPosts = new GroupIdAndPosts()
        {
            GroupId = id,
            Posts = (await _postsRepository.GetAllAsync()).Where(post => post.GroupId == id)
        };

        return View(idAndPosts);
    }

    public async Task<IActionResult> AddGroupToSubject(Guid id, string name)
    {
        Group group = new Group()
        {
            Name = name,
            SubjectId = id,
            Subject = await _subjectsRepository.GetByIdAsync(id)
        };

        await _groupsRepository.AddAsync(group);

        return Redirect($"/Home/Index");
    }

    public async Task<IActionResult> DeleteGroupFromSubject(Guid id)
    {
        await _groupsRepository.DeleteAsync(id);

        return Redirect($"/Home/Index");
    }
}