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


        // V2 PRA BAIXO
        CreateMap<RequestRegisterPersonV2Json, Person>()
            .ForMember(d => d.Id, m => m.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.CreatedAt, m => m.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.UpdatedAt, m => m.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.Street, m => m.MapFrom(s => s.Address.Street))
            .ForMember(d => d.Number, m => m.MapFrom(s => s.Address.Number))
            .ForMember(d => d.City, m => m.MapFrom(s => s.Address.City))
            .ForMember(d => d.State, m => m.MapFrom(s => s.Address.State))
            .ForMember(d => d.ZipCode, m => m.MapFrom(s => s.Address.ZipCode));
    }
}
