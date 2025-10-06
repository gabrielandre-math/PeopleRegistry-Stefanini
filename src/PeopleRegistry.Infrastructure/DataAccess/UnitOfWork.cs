using PeopleRegistry.Domain.Repositories;

namespace PeopleRegistry.Infrastructure.DataAccess;
public class UnitOfWork : IUnitOfWork
{
    private readonly PeopleRegistryDbContext _dbContext;
    public UnitOfWork(PeopleRegistryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
