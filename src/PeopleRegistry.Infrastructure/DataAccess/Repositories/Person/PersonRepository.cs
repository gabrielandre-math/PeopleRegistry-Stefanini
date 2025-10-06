using Microsoft.EntityFrameworkCore;
using Person = PeopleRegistry.Domain.Entities.Person;

namespace PeopleRegistry.Infrastructure.DataAccess.Repositories;

public class PersonRepository : IPersonReadOnlyRepository, IPersonWriteOnlyRepository, IPersonUpdateOnlyRepository
{
    private readonly PeopleRegistryDbContext _dbContext;
    public PersonRepository(PeopleRegistryDbContext dbContext) { _dbContext = dbContext; }

    public async Task Add(Person person) => await _dbContext.People.AddAsync(person);

    public async Task<bool> ExistsWithCpf(string cpf) => await _dbContext.People.AnyAsync(p => p.Cpf == cpf);

    public async Task<IList<Person>> GetAll() => await _dbContext.People.AsNoTracking().ToListAsync();

    public async Task<Person?> GetById(Guid id) => await _dbContext.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    
    public async Task<Person?> GetByIdTracked(Guid id) => await _dbContext.People.FirstOrDefaultAsync(p => p.Id == id);

    public void Update(Person person) => _dbContext.People.Update(person);

    public async Task<bool> Delete(Guid id)
    {
        var person = await _dbContext.People.FindAsync(id);
        if (person is null) return false;
        _dbContext.People.Remove(person);
        return true;
    }
}

