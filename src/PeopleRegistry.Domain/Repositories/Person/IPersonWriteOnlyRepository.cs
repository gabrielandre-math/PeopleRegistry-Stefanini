
namespace PeopleRegistry.Domain.Repositories.Person;
public interface IPersonWriteOnlyRepository
{
    Task Add(Entities.Person person);
    Task<bool> Delete(Guid id);
}
