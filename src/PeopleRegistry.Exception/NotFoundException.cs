using System.Net;

namespace PeopleRegistry.Exception;
public class NotFoundException : PeopleRegistryException
{
    public NotFoundException(string message) : base(message) { }
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
