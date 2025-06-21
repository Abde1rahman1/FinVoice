using FinVoice.Contracts.Auth;

namespace FinVoice.Services.Auth;

public interface IAuthService
{
    Task <AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken=default);
}
