using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Security.Tokens;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Exception;

public class RefreshTokenUseCase
{
    private readonly IRefreshTokenRepository _rtRepo;
    private readonly IUserRepository _userRepo;
    private readonly IAccessTokenGenerator _accessGen;
    private readonly IRefreshTokenService _rtService;
    private readonly IUnitOfWork _uow;

    public RefreshTokenUseCase(IRefreshTokenRepository rtRepo, IUserRepository userRepo,
        IAccessTokenGenerator accessGen, IRefreshTokenService rtService, IUnitOfWork uow)
    { _rtRepo = rtRepo; _userRepo = userRepo; _accessGen = accessGen; _rtService = rtService; _uow = uow; }

    public async Task<ResponseAuthJson> Execute(Guid userId, string plainRt, string? ip = null, string? ua = null, int refreshTtlDays = 30)
    {
        var token = await _rtRepo.FindByPlainTokenForUser(userId, plainRt);
        if (token is null || !token.IsActive) throw new UnauthorizedException(ResourceErrorMessages.INVALID_REFRESH_TOKEN);

        var user = await _userRepo.GetById(userId);
        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        // Rotaciona
        token.RevokedAt = DateTime.UtcNow;
        var newAccess = _accessGen.Generate(user);
        var (newPlain, newEntity) = await _rtService.Issue(user, ip, ua, refreshTtlDays);
        token.ReplacedByTokenId = newEntity.Id;

        await _rtRepo.Update(token);
        await _rtRepo.Add(newEntity);
        await _uow.Commit();

        return new ResponseAuthJson { AccessToken = newAccess, RefreshToken = newPlain };
    }
}
