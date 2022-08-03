using mednik.Models;

namespace mednik.Data.Repositories.Subjects;

public interface ISubjectsRepository
{
    Task<IEnumerable<Subject>> GetAllAsync();

    Task<Subject?> GetByIdAsync(Guid id);

    Task<bool> AddAsync(Subject subject);

    Task<bool> DeleteAsync(Guid id);
}