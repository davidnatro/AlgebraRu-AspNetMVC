namespace mednik.Data.Base;

public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(Guid id);

    Task<bool> AddAsync(T entity);

    Task<bool> Update(Guid id, T newEntity);

    Task<bool> Delete(Guid id);
}