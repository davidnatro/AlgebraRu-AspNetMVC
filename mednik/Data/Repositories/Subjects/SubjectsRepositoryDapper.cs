using System.Data;
using Dapper;
using mednik.Data.Repositories.Groups;
using mednik.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace mednik.Data.Repositories.Subjects;

public class SubjectsRepositoryDapper : ISubjectsRepository
{
    private readonly string _connectionString;

    private readonly IGroupsRepository _groupsRepository;

    public SubjectsRepositoryDapper(IOptions<MsSqlSettings> msSqlSettings, IGroupsRepository groupsRepository)
    {
        _connectionString = msSqlSettings.Value.ConnectionString;

        _groupsRepository = groupsRepository;
    }

    public async Task<IEnumerable<Subject>> GetAllAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Subjects";
            return await connection.QueryAsync<Subject>(sql);
        }
    }

    public async Task<Subject?> GetByIdAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM Subjects WHERE Id = @id";
            return await connection.QueryFirstOrDefaultAsync<Subject>(sql, new {id});
        }
    }

    public async Task<bool> AddAsync(Subject subject)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "INSERT INTO Subjects (Id, Name) VALUES (@Id, @Name)";
            await connection.ExecuteAsync(sql, subject);

            return true;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var groups = await _groupsRepository.GetAllBySubjectIdAsync(id);
            foreach (var group in groups)
            {
                await _groupsRepository.DeleteByIdAsync(group.Id);
            }

            const string sql = "DELETE FROM Subjects Where Id = @id";
            await connection.ExecuteAsync(sql, new {id});

            return true;
        }
    }
}