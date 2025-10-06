using PeopleRegistry.Domain.Entities;

namespace PeopleRegistry.Domain.Repositories.Users;

public interface IRefreshTokenService
{
    string GeneratePlainToken();
    Task<(string plainToken, RefreshToken entity)> Issue(Entities.User user, string? ip, string? userAgent, int ttlDays);
}
