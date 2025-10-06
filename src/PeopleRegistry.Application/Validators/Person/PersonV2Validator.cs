using FluentValidation;
using PeopleRegistry.Communication.Requests;

namespace PeopleRegistry.Application.Validators.Person;

public class PersonV2Validator : AbstractValidator<RequestRegisterPersonV2Json>
{
    public PersonV2Validator()
    {
        Include(new PersonValidator()); 

        RuleFor(x => x.Address).NotNull().WithMessage("Address is required.");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address.Street).NotEmpty().WithMessage("Street is required.");
            RuleFor(x => x.Address.Number).NotEmpty().WithMessage("Number is required.");
            RuleFor(x => x.Address.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Address.State).NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.Address.ZipCode).NotEmpty().WithMessage("ZipCode is required.");
        });
    }
}
