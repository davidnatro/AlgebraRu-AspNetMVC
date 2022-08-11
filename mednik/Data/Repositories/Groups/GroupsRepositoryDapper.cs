using System.Data;
using Dapper;
using mednik.Data.Repositories.Posts;
using mednik.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace mednik.Data.Repositories.Groups;

public class GroupsRepositoryDapper : IGroupsRepository
{
    private readonly string _connectionString;

    private readonly IPostsRepository _postsRepository;

    public GroupsRepositoryDapper(IOptions<MsSqlSettings> msSqlSettings, IPostsRepository postsRepository)
    {
        _connectionString = msSqlSettings.Value.ConnectionString;

        _postsRepository = postsRepository;
    }
    
    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Groups";
            return await connection.QueryAsync<Group>(sql);
        }
    }

    public async Task<IEnumerable<Group>> GetAllBySubjectIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Groups WHERE SubjectId = @id";
            return await connection.QueryAsync<Group>(sql, new {id});
        }
    }

    public async Task<Group?> GetByIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Groups WHERE Id = @id";
            return await connection.QueryFirstOrDefaultAsync<Group>(sql, new {id});
        }
    }

    public async Task<bool> AddAsync(Group group)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "INSERT INTO Groups (Id, Name, SubjectId) VALUES (@Id, @Name, @SubjectId)";
            await connection.ExecuteAsync(sql, group);

            return true;
        }
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var posts = await _postsRepository.GetAllByGroupIdAsync(id);
            foreach (var post in posts)
            {
                await _postsRepository.DeleteFileAsync(post.Id);
            }

            const string sql = "DELETE FROM Groups Where Id = @id";
            await connection.ExecuteAsync(sql, new {id});

            return true;
        }
    }
}