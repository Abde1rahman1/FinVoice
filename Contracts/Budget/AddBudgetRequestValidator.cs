using FinVoice.Contracts.ManualExpense;
using FluentValidation;

namespace FinVoice.Contracts.Budget;

public class AddBudgetRequestValidator : AbstractValidator<AddBudgetRequest>
{
    public AddBudgetRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.MonthlyLimit)
            .NotEmpty();

        RuleFor(x => x.CreatedAt)
            .NotEmpty();
    }
}
