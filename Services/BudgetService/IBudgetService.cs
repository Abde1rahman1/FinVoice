using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.Budget;

namespace FinVoice.Services.BudgetService;

public interface IBudgetService
{
    Task<Result> AddBudgetAsync(AddBudgetRequest request, string userId, CancellationToken cancellationToken);

    Task<Result<List<BudgetResponse>>> GetUserBudgetAsync(string userId);

    Task<Result> UpdateBudgetAsync(AddBudgetRequest request, string userId, int budgetId);

    Task<Result> DeleteBudgetAsync(string userId, int budgetId);

    Task<Result<BudgetComparisonResponse?>> CompareTotalSpendingToBudgetAsync(string userId);
}
