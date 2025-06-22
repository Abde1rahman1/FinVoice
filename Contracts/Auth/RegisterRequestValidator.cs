using FinVoice.Abstractions.Const;
using FluentValidation;

namespace FinVoice.Contracts.Auth;

public class RegisterRequestValidator: AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("Password should at least 8 chars , lowercase , uppercase ,and numbers");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .Length(3, 30);
    }
}
