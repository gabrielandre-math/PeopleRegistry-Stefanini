using Microsoft.EntityFrameworkCore;
using PeopleRegistry.Domain.Entities;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Domain.Security.Cryptography;

namespace PeopleRegistry.Infrastructure.DataAccess.Repositories.User;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly PeopleRegistryDbContext _db;
    private readonly IPasswordEncrypter _hasher;

    public RefreshTokenRepository(PeopleRegistryDbContext db, IPasswordEncrypter hasher)
    { _db = db; _hasher = hasher; }

    public async Task Add(RefreshToken token) => await _db.RefreshTokens.AddAsync(token);

    public async Task<RefreshToken?> GetById(Guid tokenId) =>
        await _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tokenId);

    public async Task<RefreshToken?> GetActiveByUser(Guid userId) =>
        await _db.RefreshTokens.AsNoTracking()
            .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();

    public async Task<RefreshToken?> FindByPlainTokenForUser(Guid userId, string plainToken)
    {
        var candidates = await _db.RefreshTokens
            .Where(t => t.UserId == userId && t.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(t => t.CreatedAt)
            .Take(15)
            .ToListAsync();

        return candidates.FirstOrDefault(c => _hasher.Verify(plainToken, c.TokenHash));
    }

    public async Task Update(RefreshToken token)
    {
        _db.RefreshTokens.Update(token);
        await Task.CompletedTask;
    }
}
