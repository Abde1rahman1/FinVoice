using FinVoice.Abstractions.Errors;
using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.ManualExpense;
using FinVoice.Database;
using FinVoice.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinVoice.Services.ExpenseService
{
    public class ManualExpenseService(ApplicationDbContext context):IManualExpenseService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddExpenseManualAsync(ManualExpenseRequest request, string userId, CancellationToken cancellationToken= default)
        {
            var expense = new Expense 
            {
                UserId = userId,
                Amount = request.Amount,
                Category = request.Category,
                Note = request.Note,
                Date= request.Date
            };
            await _context.Expenses.AddAsync(expense, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
            
        public async Task<Result> UpdateExpenseManualAsync(ManualExpenseRequest request, string userId, int expenseId)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x=>x.Id == expenseId);
            if (expense is null)
                return Result.Failure(ExpenseError.InvalidExpenseId);

            if (expense.UserId != userId)
                return Result.Failure(ExpenseError.UnauthenticatedToUpdate);

            expense.Amount = request.Amount;
            expense.Category = request.Category;
            expense.Note = request.Note;
            expense.Date = request.Date;
             _context.Expenses.Update(expense);
             _context.SaveChanges();
            return Result.Success();
        }
        public async Task<Result> DeleteExpenseAsync(string userId , int expenseId)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == expenseId);
            if (expense is null)
                return Result.Failure(ExpenseError.InvalidExpenseId);

            if (expense.UserId != userId)
                return Result.Failure(ExpenseError.UnauthenticatedToUpdate);
            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return Result.Success();
        }
    }
}
