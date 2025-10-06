using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Security.Tokens;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Domain.Security.Cryptography;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.User.Login;
public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenService refreshTokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Communication.Responses.ResponseAuthJson> Execute(
        RequestLoginJson request, 
        string? ip = null, 
        string? ua = null, 
        int refreshTtlDays = 30
        )
    {
        var user = await _userRepository.GetByEmail(request.Email.Trim().ToLowerInvariant());
        if (user is null || !_passwordEncrypter.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException(ResourceErrorMessages.INVALID_CREDENTIALS);

        var access = _accessTokenGenerator.Generate(user);
        var (plainRt, rtEntity) = await _refreshTokenService.Issue(user, ip, ua, refreshTtlDays);
        await _refreshTokenRepository.Add(rtEntity);
        await _unitOfWork.Commit();

        return new Communication.Responses.ResponseAuthJson { AccessToken = access, RefreshToken = plainRt };
    }
}
