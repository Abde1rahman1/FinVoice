using FinVoice.Database;
using FinVoice.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace FinVoice.Services.Notification;

public class NotificationService(ApplicationDbContext context,IEmailSender emailSender): INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task CheckAndNotifyOverBudgetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        foreach (var user in users)
        {
            var latestBudget = await _context.Budgets
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync();

            if (latestBudget == null) continue;

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var totalSpent = await _context.Expenses
                .Where(e => e.UserId == user.Id && e.Date >= startOfMonth)
                .SumAsync(e => e.Amount);

            if (totalSpent > latestBudget.MonthlyLimit)
            {
                await SendOverBudgetAlertAsync(user.Id, totalSpent, latestBudget.MonthlyLimit);
            }
        }
    }
    public async Task SendOverBudgetAlertAsync(string userId, decimal spentAmount, decimal budgetLimit)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || string.IsNullOrWhiteSpace(user.Email)) return;

        var percentUsed = budgetLimit == 0 ? 100 : Math.Round((spentAmount / budgetLimit) * 100, 2);

        var body = EmailBodyBuilder.GenerateEmailBody("OverBudget", new Dictionary<string, string>
            {
                { "{{name}}", user.FullName ?? "User" },
                { "{{spentAmount}}", spentAmount.ToString("0.00") },
                { "{{budgetLimit}}", budgetLimit.ToString("0.00") },
                { "{{percentageUsed}}", percentUsed.ToString("0.00") }
            });

        await _emailSender.SendEmailAsync(user.Email, "🚨 Budget Limit Exceeded!", body);
    }

}
