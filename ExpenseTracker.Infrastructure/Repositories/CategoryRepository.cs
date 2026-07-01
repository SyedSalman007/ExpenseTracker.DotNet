using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Repositories;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

public class CategoryRepository: BaseRepository<Category>, ICategoryRepository
{
    CategoryRepository(AppDbContext context) : base(context){}

    public async Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId) =>
        await _dbSet.Where(c => c.UserId == userId).ToListAsync();
}
