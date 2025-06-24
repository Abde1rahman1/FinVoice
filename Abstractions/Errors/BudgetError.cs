namespace FinVoice.Abstractions.Errors;

public static class BudgetError
{
    public static readonly Error NoBudgets =
    new("User.NoBudgets", "there is no budgets yet", StatusCodes.Status404NotFound);

    public static readonly Error InvalidBudgetId =
    new("User.InvalidBudgetId", "there is no budgets yet", StatusCodes.Status404NotFound);

    public static readonly Error UnauthenticatiedUser =
    new("User.UnauthenticatiedUser", "there is no budgets yet", StatusCodes.Status401Unauthorized);
}
