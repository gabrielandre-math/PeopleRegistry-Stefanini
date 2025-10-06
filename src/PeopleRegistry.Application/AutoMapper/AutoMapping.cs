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
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());

        CreateMap<RequestRegisterPersonV2Json, Person>()
            .IncludeBase<RequestRegisterPersonJson, Person>()
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address != null ? s.Address.Street : null))
            .ForMember(d => d.Number, o => o.MapFrom(s => s.Address != null ? s.Address.Number : null))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address != null ? s.Address.City : null))
            .ForMember(d => d.State, o => o.MapFrom(s => s.Address != null ? s.Address.State : null))
            .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.Address != null ? s.Address.ZipCode : null));

       
        CreateMap<Person, ResponseShortPersonJson>();
        CreateMap<Person, ResponsePersonJson>();
    }
}
