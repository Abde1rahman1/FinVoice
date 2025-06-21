namespace FinVoice.Contracts.Auth;

public record LoginRequest
(
    string email,
    string password
);
