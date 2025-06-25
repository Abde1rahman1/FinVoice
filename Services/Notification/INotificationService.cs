namespace FinVoice.Services.Notification;

public interface INotificationService
{
    Task SendOverBudgetAlertAsync(string userId, decimal spentAmount, decimal budgetLimit);
    Task CheckAndNotifyOverBudgetUsersAsync();
}
