using mednik.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace mednik.Data.Posts;

public interface IPostsRepository
{
    Task<IEnumerable<Post>> GetAllAsync();

    Task DeleteFileAsync(Guid id);

    Task UploadFile(string name, string description, IFormFile file);
    
    Task<FileStreamResult> DownloadFile(ObjectId id);
}