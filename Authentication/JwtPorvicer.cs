using FinVoice.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinVoice.Authentication;


public class JwtPorvicer(IConfiguration configuration) : IJwtPorvicer
{
    public (string token, int expiresIn) GenerateToken(User user)
    {
        Claim[] claims = [
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.GivenName, user.FullName!),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var signingCredentials = new SigningCredentials (symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var expiresIn = 30;

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresIn*60),
            signingCredentials: signingCredentials
            );


        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn);
    }
}
