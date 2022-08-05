using mednik.Data.Repositories.Posts;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class HomeController : Controller
{
    private readonly IPostsRepository _postsRepository;

    public HomeController(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var posts = (await _postsRepository.GetAllAsync()).Where(post => post.GroupId == null);
        
        return View(posts);
    }
}