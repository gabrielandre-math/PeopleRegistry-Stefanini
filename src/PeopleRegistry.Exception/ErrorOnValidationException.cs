using System.Net;

namespace PeopleRegistry.Exception;

public class ErrorOnValidationException : PeopleRegistryException
{
    private readonly List<string> _errors;

    public ErrorOnValidationException(string message, List<string> errorMessages) : base(message)
    {
        _errors = errorMessages;
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;

    public List<string> GetErrors() => _errors;
}
