
using PeopleRegistry.Domain.Entities;

public interface IPersonWriteOnlyRepository
{
    Task Add(Person person);
    Task<bool> Delete(Guid id);
}
