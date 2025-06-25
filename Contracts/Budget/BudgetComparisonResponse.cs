namespace FinVoice.Contracts.Budget;

public record BudgetComparisonResponse
(
    decimal BudgetLimit,
    decimal SpentAmount,
    double PercentageUsed,
    bool IsLimitExceeded
);
