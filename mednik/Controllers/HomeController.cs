using mednik.Data.Cache;
using mednik.Data.Repositories.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IPostsRepository _postsRepository;

    public HomeController(IPostsRepository postsRepository)
        => _postsRepository = postsRepository;


    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var posts = CachedData.CachedPosts ?? await _postsRepository.GetAllByGroupIdAsync(null);

        return View(posts);
    }
}