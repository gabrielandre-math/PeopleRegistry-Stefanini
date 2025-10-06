using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Exception;

public class LogoutUseCase
{
    private readonly IRefreshTokenRepository _rtRepo;
    private readonly IUnitOfWork _uow;

    public LogoutUseCase(IRefreshTokenRepository rtRepo, IUnitOfWork uow)
    { _rtRepo = rtRepo; _uow = uow; }

    public async Task Execute(Guid userId, string plainRt)
    {
        var token = await _rtRepo.FindByPlainTokenForUser(userId, plainRt);
        if (token is null || !token.IsActive) throw new UnauthorizedException(ResourceErrorMessages.INVALID_REFRESH_TOKEN);

        token.RevokedAt = DateTime.UtcNow;
        await _rtRepo.Update(token);
        await _uow.Commit();
    }
}
