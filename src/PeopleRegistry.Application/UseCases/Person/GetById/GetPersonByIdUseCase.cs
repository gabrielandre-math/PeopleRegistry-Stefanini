using AutoMapper;
using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.Person.GetById;

public class GetPersonByIdUseCase
{
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IMapper _mapper;

    public GetPersonByIdUseCase(IPersonReadOnlyRepository personReadOnlyRepository, IMapper mapper)
    {
        _personReadOnlyRepository = personReadOnlyRepository;
        _mapper = mapper;
    }

    public async Task<ResponsePersonJson> Execute(Guid id)
    {
        var personEntity = await _personReadOnlyRepository.GetById(id);
        if (personEntity is null)
        {
            var message = string.Format(ResourceErrorMessages.PERSON_NOT_FOUND, id);
            throw new NotFoundException(message);
        }
        return _mapper.Map<ResponsePersonJson>(personEntity);
    }
}