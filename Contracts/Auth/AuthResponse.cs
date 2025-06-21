namespace FinVoice.Contracts.Auth;

public record AuthResponse
(
    string Id,
    string Email,
    string FullName,
    String token,
    int expiresIn
 );