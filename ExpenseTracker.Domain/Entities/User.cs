using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class User: BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime LastLoginAt { get; set; }
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}