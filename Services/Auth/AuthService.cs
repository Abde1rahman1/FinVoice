using FinVoice.Abstractions.Errors;
using FinVoice.Abstractions.ResultPattern;
using FinVoice.Authentication;
using FinVoice.Contracts.Auth;
using FinVoice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace FinVoice.Services.Auth;

public class AuthService(UserManager<User> userManager,
                        SignInManager<User> signInManager,
                        IJwtPorvicer jwtPorvicer,
                        ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IJwtPorvicer _jwtPorvicer = jwtPorvicer;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false,false);

        if (result.Succeeded)
        {
            var isValidPassword = await _userManager.CheckPasswordAsync(user!, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials); ;

            var (token, expiresIn) = _jwtPorvicer.GenerateToken(user!);
            var response = new AuthResponse(user.Id, user.Email!, user.FullName, token, expiresIn);
            return Result.Success(response);
        }


        return Result.Failure<AuthResponse>(result.IsNotAllowed? UserError.EmailNotConfirmed: UserError.InvalidCredentials);
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var userEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userEmail is not null)
            return Result.Failure<AuthResponse>(UserError.emailIsExist);

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Congirmation Code {code}", code);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if(await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserError.InvalidCode);
        if (user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmation);
        
        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserError.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmation);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Congirmation Code {code}", code);

        return Result.Success();
    }
}
    