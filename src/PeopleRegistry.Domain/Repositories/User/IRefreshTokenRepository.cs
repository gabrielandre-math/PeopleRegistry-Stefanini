using PeopleRegistry.Domain.Entities;

namespace PeopleRegistry.Domain.Repositories.Users;

public interface IRefreshTokenRepository
{
    Task Add(RefreshToken token);
    Task<RefreshToken?> GetById(Guid tokenId);
    Task<RefreshToken?> GetActiveByUser(Guid userId);
    Task<RefreshToken?> FindByPlainTokenForUser(Guid userId, string plainToken);
    Task Update(RefreshToken token);
}
