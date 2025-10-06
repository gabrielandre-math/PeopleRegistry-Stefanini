namespace PeopleRegistry.Domain.Repositories.Security.Tokens;
public interface IAccessTokenGenerator
{
    string Generate(Entities.User user);
}
