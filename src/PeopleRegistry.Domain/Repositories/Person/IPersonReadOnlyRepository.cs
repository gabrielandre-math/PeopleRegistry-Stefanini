using PeopleRegistry.Domain.Entities;

public interface IPersonReadOnlyRepository
{
    Task<IList<Person>> GetAll();
    Task<Person?> GetById(Guid id);
    Task<bool> ExistsWithCpf(string cpf);
}
