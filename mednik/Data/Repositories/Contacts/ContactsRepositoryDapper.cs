using System.Data;
using Dapper;
using mednik.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace mednik.Data.Repositories.Contacts;

public class ContactsRepositoryDapper : IContactsRepository
{
    private readonly string _connectionString;

    public ContactsRepositoryDapper(IOptions<MsSqlSettings> msSqlSettings)
    {
        _connectionString = msSqlSettings.Value.ConnectionString;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "SELECT * FROM AspNetUsers";
            return await connection.QueryAsync<User>(sql);
        }
    }

    public Task ChangeAvatar(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeData(string id, string name, string telegram)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string sql = "UPDATE AspNetUsers SET FullName = @name, Telegram = @telegram WHERE Id = @id";
            await connection.ExecuteAsync(sql, new {id, name, telegram});
        }
    }
}