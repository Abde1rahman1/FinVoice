
using FluentValidation;

namespace FinVoice.Contracts.Auth;

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.email)
             .NotEmpty()
             .EmailAddress();
    }
}
