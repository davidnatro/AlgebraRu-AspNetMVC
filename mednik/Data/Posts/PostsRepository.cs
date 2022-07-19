using mednik.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace mednik.Data.Posts;

public class PostsRepository : IPostsRepository
{
    private readonly IGridFSBucket _gridFsBucket;

    private readonly AppDbContext _dbContext;

    public PostsRepository(IOptions<MongoDBSettings> mongoDBSettings, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);

        var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

        _gridFsBucket = new GridFSBucket(database);
    }

    public async Task<IEnumerable<Post>> GetAllAsync() => await _dbContext.Posts.ToListAsync();

    public async Task UploadFile(string name, string description, IFormFile file)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            
            var id = await _gridFsBucket.UploadFromStreamAsync(name, stream);
            
            Post post = new Post() {Name = name, Description = description, ImageURL = id.ToString()};
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> DownloadFile(ObjectId id)
    {
        throw new NotImplementedException();
    }
    
    public async Task DeleteFileAsync(Guid id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        
        if (post != null)
        {
            try
            {
                var imgUrl = post.ImageURL;
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();

                var objectId = new ObjectId(imgUrl);

                await _gridFsBucket.DeleteAsync(objectId);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}