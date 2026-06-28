using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class Category: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}