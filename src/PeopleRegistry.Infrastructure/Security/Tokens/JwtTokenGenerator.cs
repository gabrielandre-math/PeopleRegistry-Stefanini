using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PeopleRegistry.Domain.Repositories.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeopleRegistry.Infrastructure.Security.Tokens;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationInMinutes;
    private readonly string _signingKey;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _signingKey = configuration["Jwt:SigningKey"]!;
                _expirationInMinutes = uint.Parse(configuration["Jwt:ExpirationTimeMinutes"]!);
    }

    public string Generate(Domain.Entities.User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_expirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}