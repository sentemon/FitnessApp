namespace PostService.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IQueryable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(Guid id);
}