namespace PeopleRegistry.Domain.Repositories.Person;

public interface IPersonUpdateOnlyRepository
{
    Task<Entities.Person?> GetByIdTracked(Guid id);
    void Update(Entities.Person person);
}
