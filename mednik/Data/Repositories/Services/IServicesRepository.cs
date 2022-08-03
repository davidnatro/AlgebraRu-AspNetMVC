namespace mednik.Data.Repositories.Services;

public interface IServicesRepository
{
    Task<IEnumerable<Models.Services>> GetAllAsync();

    Task<bool> AddAsync(Models.Services entity);

    Task<bool> DeleteAsync(Guid id);
}