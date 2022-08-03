using Microsoft.EntityFrameworkCore;

namespace mednik.Data.Repositories.Services;

public class ServicesRepository : IServicesRepository
{
    private readonly AppDbContext _dbContext;

    public ServicesRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Models.Services>> GetAllAsync() => await _dbContext.Services.ToListAsync();

    public async Task<bool> AddAsync(Models.Services entity)
    {
        try
        {
            await _dbContext.Services.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return false;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _dbContext.Services.FirstOrDefaultAsync(service => service.Id == id);
            if (entity != null)
            {
                _dbContext.Services.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }
}