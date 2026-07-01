using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Repositories;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId) =>
        await _dbSet.Where(e => e.UserId == userId).ToListAsync();

    public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(Guid categoryId) =>
        await _dbSet.Where(e => e.CategoryId == categoryId).ToListAsync();
}
