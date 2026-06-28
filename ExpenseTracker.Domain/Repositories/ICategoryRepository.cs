using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Repositories;

public interface ICategoryRepository: IRepository<Category>
{
    Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId);
}