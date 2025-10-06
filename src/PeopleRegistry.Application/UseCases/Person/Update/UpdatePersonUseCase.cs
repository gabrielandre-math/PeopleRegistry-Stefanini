using AutoMapper;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.Person.Update;
public class UpdatePersonUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonUpdateOnlyRepository _updateOnlyRepository;

    public UpdatePersonUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IPersonUpdateOnlyRepository updateOnlyRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _updateOnlyRepository = updateOnlyRepository;
    }

    public async Task Execute(Guid id, RequestRegisterPersonJson request)
    {
        var personEntity = await _updateOnlyRepository.GetByIdTracked(id);
        
        if (personEntity is null)
        {
            var message = string.Format(ResourceErrorMessages.PERSON_NOT_FOUND, id);
            throw new NotFoundException(message);
        }
        _mapper.Map(request, personEntity);
        personEntity.UpdatedAt = DateTime.UtcNow;

        _updateOnlyRepository.Update(personEntity);
        await _unitOfWork.Commit();
    }
}
