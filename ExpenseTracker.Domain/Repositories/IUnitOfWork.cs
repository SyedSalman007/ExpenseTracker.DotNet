namespace ExpenseTracker.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}