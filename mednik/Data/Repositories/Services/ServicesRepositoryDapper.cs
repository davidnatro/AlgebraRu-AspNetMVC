using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace mednik.Data.Repositories.Services;

public class ServicesRepositoryDapper : IServicesRepository
{
    private readonly string _connectionString;

    public ServicesRepositoryDapper(IOptions<MsSqlSettings> msSqlSettings)
    {
        _connectionString = msSqlSettings.Value.ConnectionString;
    }

    public async Task<IEnumerable<Models.Services>> GetAllAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            const string sql = "SELECT * FROM Services";
            return await connection.QueryAsync<Models.Services>(sql);
        }
    }

    public async Task<bool> AddAsync(Models.Services entity)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "INSERT INTO Services (Id, Name, Link) VALUES (@Id, @Name, @Link)";
            await connection.ExecuteAsync(sql, entity);

            return true;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            const string sql = "DELETE FROM Services Where Id = @id";
            await connection.ExecuteAsync(sql, new { id });

            return true;
        }
    }
}