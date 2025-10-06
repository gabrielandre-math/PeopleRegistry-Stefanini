// PeopleRegistry.Application/AutoMapper/AutoMapping.cs
using AutoMapper;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Domain.Entities;

namespace PeopleRegistry.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<RequestRegisterPersonJson, Person>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Person, ResponseShortPersonJson>();
        CreateMap<Person, ResponsePersonJson>();

        
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
