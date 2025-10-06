using System.Security.Cryptography;
using PeopleRegistry.Domain.Entities;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Domain.Security.Cryptography;

namespace PeopleRegistry.Infrastructure.Security.Tokens;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IPasswordEncrypter _hasher;
    public RefreshTokenService(IPasswordEncrypter hasher) => _hasher = hasher;

    public string GeneratePlainToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32); // 256 bits
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public async Task<(string plainToken, RefreshToken entity)> Issue(
        User user, string? ip, string? userAgent, int ttlDays)
    {
        var plain = GeneratePlainToken();

        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = _hasher.Encrypt(plain),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(ttlDays),
            CreatedByIp = ip,
            UserAgent = userAgent
        };

        await Task.CompletedTask; 
        return (plain, entity);
    }
}
