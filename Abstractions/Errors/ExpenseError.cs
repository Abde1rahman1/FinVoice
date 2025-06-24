namespace FinVoice.Abstractions.Errors;

public static class ExpenseError
{
    public static readonly Error UnauthenticatedToUpdate =
        new("User.UnauthenticatedToUpdate", "Unauthenticated To Update Expense", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidExpenseId =
        new("User.InvalidExpenseId", "Invalid Expense Id, try valid one", StatusCodes.Status404NotFound);
}
