namespace PeopleRegistry.Domain.Repositories.Person;

public interface IPersonReadOnlyRepository
{
    Task<IList<Entities.Person>> GetAll();
    Task<Entities.Person?> GetById(Guid id);
    Task<bool> ExistsWithCpf(string cpf);
}
