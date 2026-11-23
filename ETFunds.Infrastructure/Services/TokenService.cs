using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ETFunds.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace ETFunds.Infrastructure.Services;

public class TokenService(IConfiguration _config)
{


    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:ExpiryMinutes"]!)
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}