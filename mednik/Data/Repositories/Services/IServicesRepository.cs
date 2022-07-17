namespace mednik.Data.Repositories.Services;

public interface IServicesRepository
{
    Task<IEnumerable<Models.Services>> GetAllAsync();

    Task<Models.Services?> GetByIdAsync(Guid id);

    Task<bool> AddAsync(Models.Services entity);

    Task<bool> UpdateAsync(Guid id, Models.Services newEntity);

    Task<bool> DeleteAsync(Guid id);
}