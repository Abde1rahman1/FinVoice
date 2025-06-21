using Microsoft.AspNetCore.Identity;

namespace FinVoice.Entities;

public sealed class User:IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public ICollection<Expense> Expenses { get; set; }

    public ICollection<Budget> Budgets { get; set; }
}
