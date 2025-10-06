using System.Net;

namespace PeopleRegistry.Exception;

public class UnauthorizedException : PeopleRegistryException
{
    public UnauthorizedException(string message) : base(message) { }
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
