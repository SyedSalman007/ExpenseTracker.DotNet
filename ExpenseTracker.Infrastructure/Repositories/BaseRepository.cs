using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Repositories;
using ExpenseTracker.Infrastructure.Data;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

public class BaseRepository<T>: IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    
    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ICollection<T>> GetAllAsync() => 
        await _dbSet.ToListAsync();

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);
    
    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
        }
    }
}