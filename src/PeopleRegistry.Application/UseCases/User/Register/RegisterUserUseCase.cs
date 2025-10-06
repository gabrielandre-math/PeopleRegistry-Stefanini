using AutoMapper;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Security.Tokens;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Domain.Security.Cryptography;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public RegisterUserUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,  
        IRefreshTokenService refreshTokenService)        
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository; 
        _refreshTokenService = refreshTokenService;       
    }

    public async Task<Communication.Responses.ResponseAuthJson> Execute(
        RequestRegisterUserJson request,
        string? ip = null,
        string? ua = null,
        int refreshTtlDays = 30)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmail(normalizedEmail);
        if (existingUser is not null)
            throw new ConflictException(ResourceErrorMessages.EMAIL_IN_USE);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Email = normalizedEmail;
        user.PasswordHash = _passwordEncrypter.Encrypt(request.Password);

        await _userRepository.Add(user);
        await _unitOfWork.Commit();

        var access = _accessTokenGenerator.Generate(user);
        var (plainRt, rtEntity) = await _refreshTokenService.Issue(user, ip, ua, refreshTtlDays);
        await _refreshTokenRepository.Add(rtEntity);
        await _unitOfWork.Commit();

        return new Communication.Responses.ResponseAuthJson { AccessToken = access, RefreshToken = plainRt };
    }
}
