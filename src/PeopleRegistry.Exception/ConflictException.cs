using System.Net;

namespace PeopleRegistry.Exception;

public class ConflictException : PeopleRegistryException
{
    public ConflictException(string message) : base(message) { }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;
}