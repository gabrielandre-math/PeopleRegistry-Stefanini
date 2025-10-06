using FluentValidation;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Exception;
using System.Text.RegularExpressions;

namespace PeopleRegistry.Application.Validators.Person;
public class PersonValidator : AbstractValidator<RequestRegisterPersonJson>
{
    public PersonValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(ResourceErrorMessages.NAME_REQUIRED);

        RuleFor(p => p.DateOfBirth)
            .NotEmpty().WithMessage(ResourceErrorMessages.DATEOFBIRTH_REQUIRED)
            .LessThan(DateTime.Now).WithMessage(ResourceErrorMessages.DATEOFBIRTH_FUTURE);

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage(ResourceErrorMessages.CPF_REQUIRED)
            .Must(cpf =>
            {
                if (string.IsNullOrEmpty(cpf)) return false;
                var cleanCpf = Regex.Replace(cpf, @"[^\d]", "");
                return cleanCpf.Length == 11;
            }).WithMessage(ResourceErrorMessages.CPF_INVALID_LENGTH);

        When(p => !string.IsNullOrEmpty(p.Email), () =>
        {
            RuleFor(p => p.Email)
                .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        });
    }
}
