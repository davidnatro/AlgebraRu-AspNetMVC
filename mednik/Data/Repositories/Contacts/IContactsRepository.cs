using mednik.Models;

namespace mednik.Data.Repositories.Contacts;

public interface IContactsRepository
{
    Task<IEnumerable<User>> GetAllAsync();

    Task ChangeAvatar(IFormFile file);

    Task ChangeData(string id, string name, string telegram);
}