using FluentValidation;

namespace FinVoice.Contracts.ManualExpense;

public class ManualExpenseValidator:AbstractValidator<ManualExpenseRequest>
{
    public ManualExpenseValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty();

        RuleFor(x=>x.Category)
            .NotEmpty();
        
        RuleFor(x=>x.Date)
            .NotEmpty();
    }

}
