using mednik.Models;

namespace mednik.Data.Repositories.Groups;

public interface IGroupsRepository
{
    Task<IEnumerable<Group>> GetAllAsync();
    
    Task<IEnumerable<Group>> GetAllBySubjectIdAsync(Guid id);

    Task<Group?> GetByIdAsync(Guid id);

    Task<bool> AddAsync(Group group);

    Task<bool> DeleteAsync(Guid id);
}