using AutoMapper;
using PeopleRegistry.Application.Validators.Person;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Domain.Repositories;
using PeopleRegistry.Domain.Repositories.Person;
using PeopleRegistry.Exception;

namespace PeopleRegistry.Application.UseCases.Person.Register;

public class RegisterPersonUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonWriteOnlyRepository _writeOnlyRepository;
    private readonly IPersonReadOnlyRepository _readOnlyRepository;

    public RegisterPersonUseCase(
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IPersonWriteOnlyRepository writeOnlyRepository, 
        IPersonReadOnlyRepository readOnlyRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<ResponseRegisteredPersonJson> Execute(RequestRegisterPersonJson request)
    {
        await Validate(request);

        var personEntity = _mapper.Map<Domain.Entities.Person>(request);

        await _writeOnlyRepository.Add(personEntity);
        await _unitOfWork.Commit();

        return new ResponseRegisteredPersonJson { Id = personEntity.Id };
    }

    private async Task Validate(RequestRegisterPersonJson request)
    {
        var validator = new PersonValidator();
        var result = await validator.ValidateAsync(request);

        var cpfExists = await _readOnlyRepository.ExistsWithCpf(request.Cpf);
        if (cpfExists)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("Cpf", ResourceErrorMessages.CPF_ALREADY_EXISTS));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(ResourceErrorMessages.VALIDATION_FAILED, errorMessages);
        }
    }
}