using mednik.Data.Repositories.Groups;
using mednik.Models;
using Microsoft.EntityFrameworkCore;

namespace mednik.Data.Repositories.Subjects;

public class SubjectsRepository : ISubjectsRepository
{
    private readonly AppDbContext _dbContext;

    private readonly IGroupsRepository _groupsRepository;

    public SubjectsRepository(AppDbContext dbContext, IGroupsRepository groupsRepository)
    {
        _dbContext = dbContext;

        _groupsRepository = groupsRepository;
    }

    public async Task<IEnumerable<Subject>> GetAllAsync() => await _dbContext.Subjects.ToListAsync();

    public async Task<Subject?> GetByIdAsync(Guid id) =>
        await _dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);

    public async Task<bool> AddAsync(Subject subject)
    {
        try
        {
            await _dbContext.AddAsync(subject);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var subject = await _dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);

        if (subject == null) return false;

        var groups = (await _groupsRepository.GetAllAsync()).Where(group => group.SubjectId == id);
        foreach (var group in groups)
        {
            await _groupsRepository.DeleteByIdAsync(group.Id);
        }
        
        _dbContext.Remove(subject);
        await _dbContext.SaveChangesAsync();

        return true;

    }
}