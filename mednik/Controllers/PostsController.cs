using mednik.Data;
using mednik.Data.Posts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace mednik.Controllers;

public class PostsController : Controller
{
    private readonly IPostsRepository _postsRepository;

    public PostsController(IPostsRepository postsRepository, AppDbContext appDbContext)
    {
        _postsRepository = postsRepository;
    }
    
    public IActionResult Index() => View();

    [HttpGet]
    public async Task<FileStreamResult> Render(string imgUrl)
    {
        var id = new ObjectId(imgUrl);

        return await _postsRepository.DownloadFile(id);
    }
    
    public async Task<IActionResult> Save(string? name, string description, IFormFile? data)
    { 
        if (name != null && data != null)
            await _postsRepository.UploadFile(name, description, data);

        return Redirect("/Home");
    }

    public async Task<IActionResult> Delete(Guid Id)
    {
        await _postsRepository.DeleteFileAsync(Id);

        return RedirectToAction("Index", "Home");
    }
}