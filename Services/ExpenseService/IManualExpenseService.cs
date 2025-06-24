using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.ManualExpense;

namespace FinVoice.Services.ExpenseService;

public interface IManualExpenseService
{
    Task<Result> AddExpenseManualAsync(ManualExpenseRequest request, string userId, CancellationToken cancellationToken = default);

    Task<Result> UpdateExpenseManualAsync(ManualExpenseRequest request, string userId, int expenseId);

    Task<Result> DeleteExpenseAsync(string userId, int expenseId);

}
