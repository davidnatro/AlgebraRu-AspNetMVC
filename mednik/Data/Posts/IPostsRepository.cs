using MongoDB.Bson;

namespace mednik.Data.Posts;

public interface IPostsRepository
{
    Task UploadFile(string name, string description, IFormFile file);
    
    Task<bool> DownloadFile(ObjectId id);
    
    Task<bool> DeleteFile(ObjectId id);
}