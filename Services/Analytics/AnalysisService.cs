using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.Analysis;
using FinVoice.Database;
using Microsoft.EntityFrameworkCore;

namespace FinVoice.Services.Analytics;

public class AnalysisService(ApplicationDbContext context) : IAnalysisService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<SpendingInsightResponse>> GetSpendingInsightAsync(string userId)
    {
        var now = DateTime.UtcNow;
        var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
        var startOfLastMonth = startOfThisMonth.AddMonths(-1);

        var currentMonthSpent = await _context.Expenses
            .Where(e => e.UserId == userId && e.Date >= startOfThisMonth)
            .SumAsync(e => e.Amount);

        var lastMonthSpent = await _context.Expenses
            .Where(e => e.UserId == userId && e.Date >= startOfLastMonth && e.Date < startOfThisMonth)
            .SumAsync(e => e.Amount);

        string insight;
        if (lastMonthSpent == 0 && currentMonthSpent == 0)
        {
            insight = "No spending recorded in the last two months.";
        }
        else if (lastMonthSpent == 0)
        {
            insight = $"You spent {currentMonthSpent} this month. No spending recorded last month.";
        }
        else
        {
            var difference = currentMonthSpent - lastMonthSpent;
            var percentage = Math.Round((difference / lastMonthSpent) * 100, 2);

            if (difference > 0)
                insight = $"You spent {percentage}% more than last month.";
            else if (difference < 0)
                insight = $"You spent {Math.Abs(percentage)}% less than last month.";
            else
                insight = "Your spending is exactly the same as last month.";
        }

        var res = new SpendingInsightResponse
        (
             currentMonthSpent,
             lastMonthSpent,
             insight
        );
        return Result.Success(res);
    }
}
