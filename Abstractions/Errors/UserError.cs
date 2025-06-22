
using Microsoft.AspNetCore.Http;
using System.Net;


namespace FinVoice.Abstractions.Errors;

public static class UserError
{
    public static readonly Error emailIsExist =
        new("User.emalIsExist", "Email is already exist !", StatusCodes.Status409Conflict);

    public static readonly Error InvalidCredentials =
           new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailNotConfirmed =
         new("User.EmailNotConfirmed", "Email Not Confirmed yet", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode =
     new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedConfirmation =
    new("User.InvalidCode", "Duplicated Confirmation", StatusCodes.Status400BadRequest);
}

