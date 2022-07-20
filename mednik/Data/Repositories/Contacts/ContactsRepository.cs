using mednik.Models;
using Microsoft.EntityFrameworkCore;

namespace mednik.Data.Repositories.Contacts;

public class ContactsRepository : IContactsRepository
{
    private readonly AppDbContext _dbContext;

    public ContactsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _dbContext.Users.ToListAsync();
    
    public Task ChangeAvatar(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeData(string id, string? name, string? telegram)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(info => info.Id == id);

        if (user != null)
        {
            user.FullName = name ?? user.FullName;
            user.Telegram = telegram ?? user.Telegram;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}