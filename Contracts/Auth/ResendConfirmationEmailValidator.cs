using FluentValidation;

namespace FinVoice.Contracts.Auth;

public class ResendConfirmationEmailValidator: AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailValidator()
    {
        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();
    }
}
