
using PeopleRegistry.Domain.Entities;

public interface IPersonUpdateOnlyRepository
{
    Task<Person?> GetByIdTracked(Guid id);
    void Update(Person person);
}
