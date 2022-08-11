using mednik.Data.Repositories.Posts;
using mednik.Data.Repositories.Subjects;
using mednik.Models;
using Microsoft.EntityFrameworkCore;

namespace mednik.Data.Repositories.Groups;

public class GroupsRepository : IGroupsRepository
{
    private readonly AppDbContext _dbContext;

    private readonly IPostsRepository _postsRepository;

    public GroupsRepository(AppDbContext dbContext, IPostsRepository postsRepository)
    {
        _dbContext = dbContext;

        _postsRepository = postsRepository;
    }

    public async Task<IEnumerable<Group>> GetAllAsync() => await _dbContext.Groups.ToListAsync();

    public async Task<IEnumerable<Group>> GetAllBySubjectIdAsync(Guid id)
        => (await _dbContext.Groups.ToListAsync()).Where(group => group.SubjectId == id);

    public async Task<Group?> GetByIdAsync(Guid id)
        => await _dbContext.Groups.FirstOrDefaultAsync(group => group.Id == id);

    public async Task<bool> AddAsync(Group group)
    {
        try
        {
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        return false;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var group = await _dbContext.Groups.FirstOrDefaultAsync(group => group.Id == id);

        if (group != null)
        {
            var posts = (await _postsRepository.GetAllAsync()).Where(post => post.GroupId == id);
            foreach (var post in posts)
            {
                await _postsRepository.DeleteFileAsync(post.Id);
            }

            _dbContext.Remove(group);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}