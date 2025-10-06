using Microsoft.Extensions.DependencyInjection;
using PeopleRegistry.Application.UseCases.Person.Delete;
using PeopleRegistry.Application.UseCases.Person.GetAll;
using PeopleRegistry.Application.UseCases.Person.GetById;
using PeopleRegistry.Application.UseCases.Person.Register;
using PeopleRegistry.Application.UseCases.Person.Update;
using PeopleRegistry.Application.UseCases.User.Login;
using PeopleRegistry.Application.UseCases.User.Register;

namespace PeopleRegistry.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapper.AutoMapping));
        services.AddScoped<RegisterPersonUseCase>();
        services.AddScoped<GetAllPeopleUseCase>();
        services.AddScoped<GetPersonByIdUseCase>();
        services.AddScoped<UpdatePersonUseCase>();
        services.AddScoped<DeletePersonUseCase>();

        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<LoginUseCase>();

        services.AddScoped<RefreshTokenUseCase>(); 
        services.AddScoped<LogoutUseCase>();

        // Injeção V2 
        services.AddScoped<RegisterPersonV2UseCase>();

    }
}