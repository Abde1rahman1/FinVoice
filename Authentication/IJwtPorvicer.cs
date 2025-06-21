using FinVoice.Entities;

namespace FinVoice.Authentication;

public interface IJwtPorvicer
{
    (string token, int expiresIn) GenerateToken(User user);
}
