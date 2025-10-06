using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Person;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.Person.Delete;
public class DeletePersonUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonWriteOnlyRepository _personWriteOnlyRepository;

    public DeletePersonUseCase(
        IUnitOfWork unitOfWork,
        IPersonWriteOnlyRepository personWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _personWriteOnlyRepository = personWriteOnlyRepository;
    }

    public async Task Execute(Guid id)
    {
        var wasDeleted = await _personWriteOnlyRepository.Delete(id);
        if (!wasDeleted)
        {
            var message = string.Format(ResourceErrorMessages.PERSON_NOT_FOUND, id);
            throw new NotFoundException(message);
        }
        await _unitOfWork.Commit();
    }
}
