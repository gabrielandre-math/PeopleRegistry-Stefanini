using AutoMapper;
using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Domain.Repositories.Person;

namespace PeopleRegistry.Application.UseCases.Person.GetAll;
public class GetAllPeopleUseCase
{
    private readonly IMapper _mapper;
    private readonly IPersonReadOnlyRepository _readOnlyRepository;

    public GetAllPeopleUseCase(IMapper mapper, IPersonReadOnlyRepository readOnlyRepository)
    {
        _mapper = mapper;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<ResponsePeopleJson> Execute()
    {
        var peopleEntities = await _readOnlyRepository.GetAll();

        var peopleList = _mapper.Map<List<ResponseShortPersonJson>>(peopleEntities);

        return new ResponsePeopleJson
        {
            People = peopleList
        };
    }
}
