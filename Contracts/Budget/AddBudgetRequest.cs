namespace FinVoice.Contracts.Budget;

public record AddBudgetRequest
(
    string Category,
    decimal MonthlyLimit,
    DateTime CreatedAt
);
