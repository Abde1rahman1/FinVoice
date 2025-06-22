﻿using FluentValidation;

namespace FinVoice.Contracts.Auth;

public class ConfirmEmailRequestValidator: AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId)
             .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
