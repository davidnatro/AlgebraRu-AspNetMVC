using mednik.Models;
using MongoDB.Bson;

namespace mednik.Data.Posts;

public interface IPostsRepository
{
    Task<IEnumerable<Post>> GetAllAsync();

    Task UploadFile(string name, string description, IFormFile file);
    
    Task<bool> DownloadFile(ObjectId id);
    
    Task<bool> DeleteFile(ObjectId id);
}