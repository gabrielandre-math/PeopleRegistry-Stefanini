using System.Net;

namespace PeopleRegistry.Exception
{
    public abstract class PeopleRegistryException : System.Exception
    {
        public PeopleRegistryException(string message) : base(message) { }
        public abstract HttpStatusCode GetStatusCode();
    }
}
