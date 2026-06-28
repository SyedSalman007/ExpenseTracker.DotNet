using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class Expense: BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    
}