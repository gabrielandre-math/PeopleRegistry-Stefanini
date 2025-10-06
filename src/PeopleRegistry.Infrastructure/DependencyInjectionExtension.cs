using Microsoft.Extensions.DependencyInjection;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Security.Tokens;
using PeopleRegistry.Domain.Repositories.Users;
using PeopleRegistry.Domain.Security.Cryptography;
using PeopleRegistry.Infrastructure.DataAccess;
using PeopleRegistry.Infrastructure.DataAccess.Repositories;
using PeopleRegistry.Infrastructure.DataAccess.Repositories.User;
using PeopleRegistry.Infrastructure.Security.Cryptography;
using PeopleRegistry.Infrastructure.Security.Tokens;

namespace PeopleRegistry.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        // Repositórios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPersonReadOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonWriteOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonUpdateOnlyRepository, PersonRepository>();

        // Serviços de segurança
        services.AddScoped<IPasswordEncrypter, BCryptEncrypter>();
        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    }
}