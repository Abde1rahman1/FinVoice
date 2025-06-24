using FinVoice.Abstractions.Errors;
using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.Budget;
using FinVoice.Database;
using FinVoice.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinVoice.Services.BudgetService;

public class BudgetService(ApplicationDbContext context) : IBudgetService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddBudgetAsync (AddBudgetRequest request,string userId ,CancellationToken cancellationToken)
    {
        var budget = new Budget
        {
            UserId = userId,
            Category = request.Category,
            CreatedAt = request.CreatedAt,
            MonthlyLimit = request.MonthlyLimit
        };
        await _context.Budgets.AddAsync(budget, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result<List<BudgetResponse>>> GetUserBudgetAsync(string userId)
    {
        var budget = await _context.Budgets
                                   .Where(x => x.UserId == userId)
                                   .ToListAsync();
        if (budget is null)
            return Result.Failure<List<BudgetResponse>>(BudgetError.NoBudgets);

        var budgetResponses = budget.Select(budget => new BudgetResponse
               (
                   Category: budget.Category,
                   CreatedAt: budget.CreatedAt,
                   MonthlyLimit: budget.MonthlyLimit,
                   Id: budget.Id,
                   userId: budget.UserId
               )).ToList();

        return Result.Success(budgetResponses);
    }
    public async Task<Result> UpdateBudgetAsync(AddBudgetRequest request, string userId, int budgetId)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);
        if (budget is null)
            return Result.Failure(BudgetError.InvalidBudgetId);

        if (budget.UserId != userId)
            return Result.Failure(BudgetError.UnauthenticatiedUser);

        budget.MonthlyLimit= request.MonthlyLimit;
        budget.Category= request.Category;
        budget.CreatedAt= request.CreatedAt;

        _context.Budgets.Update(budget);
        _context.SaveChanges();
        return Result.Success();
    }

    public async Task<Result> DeleteBudgetAsync(string userId, int budgetId)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == budgetId);
        if (budget is null)
            return Result.Failure(BudgetError.InvalidBudgetId);

        if (budget.UserId != userId)
            return Result.Failure(BudgetError.UnauthenticatiedUser);

        _context.Budgets.Remove(budget);
        _context.SaveChanges();
        return Result.Success();
    }
}
