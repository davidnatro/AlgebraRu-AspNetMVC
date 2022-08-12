using mednik.Data.Repositories.Posts;
using mednik.Models;
using mednik.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace mednik.Controllers;

[Authorize]
public class PostsController : Controller
{
    private readonly IPostsRepository _postsRepository;

    public PostsController(IPostsRepository postsRepository)
        => _postsRepository = postsRepository;


    public IActionResult Index() => View();

    [HttpGet]
    [AllowAnonymous]
    public async Task<FileStreamResult> Render(string imgUrl)
    {
        var id = new ObjectId(imgUrl);

        return await _postsRepository.DownloadFile(id);
    }

    public async Task<IActionResult> Save(PostDTO postDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Поля \"Название\" и \"Файл\" не должны быть пустыми!");
            return View("Index", postDto);
        }

        await _postsRepository.UploadFile(postDto.Name, postDto.Description, postDto.FileData);

        return Redirect("/Home");
    }

    public async Task<IActionResult> Delete(Guid Id)
    {
        await _postsRepository.DeleteFileAsync(Id);

        return RedirectToAction("Index", "Home");
    }
}