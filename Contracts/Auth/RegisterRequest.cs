namespace FinVoice.Contracts.Auth;

public record RegisterRequest
(
    string Email,
    string Password,
    string FullName,
    string PhoneNumber
 );
    