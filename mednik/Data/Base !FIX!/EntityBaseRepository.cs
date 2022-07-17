using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace mednik.Data.Base;

public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
{
    private readonly AppDbContext _dbContext;

    public EntityBaseRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

    public async Task<T> GetByIdAsync(Guid id) =>
        await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);

    public async Task<bool> AddAsync(T entity)
    {
        try
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Guid id, T newEntity)
    {
        try
        {
            _dbContext.Set<T>().Update(newEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        T entry = await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);

        if (entry != null)
        {
            _dbContext.Set<T>().Remove(entry);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}