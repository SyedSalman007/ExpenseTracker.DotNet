using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Repositories;

public interface IExpenseRepository: IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Expense>> GetByCategoryIdAsync(Guid categoryId);
}