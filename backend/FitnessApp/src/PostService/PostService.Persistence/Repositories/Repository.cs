using Microsoft.EntityFrameworkCore;
using PostService.Domain.Interfaces;

namespace PostService.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly PostDbContext _context;

    public Repository(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<TEntity>> GetAllAsync()
    {
        // ToDo: load by page index
        return  await Task.FromResult(_context.Set<TEntity>().AsNoTracking().AsQueryable());
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        
        if (entity == null)
        {
            return false;
        }

        var entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;

        await _context.SaveChangesAsync();
        
        return true;
    }
}