using mednik.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace mednik.Data.Repositories.Posts;

public interface IPostsRepository
{
    Task<IEnumerable<Post>> GetAllAsync();

    Task DeleteFileAsync(Guid id);

    Task UploadFile(string name, string description, IFormFile file, Guid? groupId = null);
    
    Task<FileStreamResult> DownloadFile(ObjectId id);
}