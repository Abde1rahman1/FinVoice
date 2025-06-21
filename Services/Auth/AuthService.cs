using FinVoice.Authentication;
using FinVoice.Contracts.Auth;
using FinVoice.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinVoice.Services.Auth;

public class AuthService(UserManager<User> userManager,IJwtPorvicer jwtPorvicer) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtPorvicer _jwtPorvicer = jwtPorvicer;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) 
            return null;

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword) return null;

        var (token, expiresIn) = _jwtPorvicer.GenerateToken(user);

        return new AuthResponse(user.Id, user.Email!, user.FullName, token, expiresIn);

    }
}
    