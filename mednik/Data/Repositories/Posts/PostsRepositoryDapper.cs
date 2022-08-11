using System.Data;
using Dapper;
using mednik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace mednik.Data.Repositories.Posts;

public class PostsRepositoryDapper : IPostsRepository
{
    private readonly IGridFSBucket _gridFsBucket;

    private readonly string _connectionString;

    public PostsRepositoryDapper(IOptions<MongoDBSettings> mongoDBSettings, IOptions<MsSqlSettings> msSqlSettings)
    {
        var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _gridFsBucket = new GridFSBucket(database);

        _connectionString = msSqlSettings.Value.ConnectionString;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Posts";
            return await connection.QueryAsync<Post>(sql);
        }
    }

    private async Task<Post> GetPostByIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Posts WHERE Id = @id";

            return await connection.QueryFirstOrDefaultAsync<Post>(sql, new {id});
        }
    }

    public async Task<IEnumerable<Post>> GetAllByGroupIdAsync(Guid? id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            string sql = id == null ? "SELECT * FROM Posts WHERE GroupId is NULL" : "SELECT * FROM Posts WHERE GroupId = @id";
            return await connection.QueryAsync<Post>(sql, new {id});
        }
    }

    private async Task DeletePostsByIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "DELETE FROM Posts WHERE Id = @id";
            await connection.ExecuteAsync(sql, new {id});
        }
    }

    public async Task UploadFile(string name, string description, IFormFile file, Guid? groupId = null)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var stream = new MemoryStream();

            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var id = await _gridFsBucket.UploadFromStreamAsync(name, stream);

            Post post = new Post()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                ImageURL = id.ToString(),
                GroupId = groupId
            };

            const string sql = "INSERT INTO Posts (Id, Name, Description, ImageURL, GroupId) VALUES " +
                               "(@Id, @Name, @Description, @ImageURL, @GroupId)";

            await connection.ExecuteAsync(sql, post);
        }
    }

    public async Task<FileStreamResult> DownloadFile(ObjectId id)
    {
        var bytes = await _gridFsBucket.DownloadAsBytesAsync(id);

        MemoryStream memoryStream = new MemoryStream();

        await memoryStream.WriteAsync(bytes, 0, bytes.Length);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(memoryStream, "application/pdf");
    }

    public async Task DeleteFileAsync(Guid id)
    {
        var post = await GetPostByIdAsync(id);

        if (post != null)
        {
            try
            {
                var imgUrl = post.ImageURL;

                await DeletePostsByIdAsync(id);

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