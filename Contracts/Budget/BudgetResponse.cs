namespace FinVoice.Contracts.Budget;

public record BudgetResponse
(
    int Id,
    string userId,
    string Category,
    decimal MonthlyLimit,
    DateTime CreatedAt
    );
