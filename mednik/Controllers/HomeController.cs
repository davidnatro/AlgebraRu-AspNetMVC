using mednik.Data;
using mednik.Data.Base;
using mednik.Data.Repositories.Posts;
using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;


public class HomeController : Controller
{
    private IPostsRepository _postsRepository;

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