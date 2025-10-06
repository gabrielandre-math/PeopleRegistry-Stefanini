using Microsoft.EntityFrameworkCore;
using PeopleRegistry.Domain.Repositories.Users;

namespace PeopleRegistry.Infrastructure.DataAccess.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly PeopleRegistryDbContext _dbContext;

    public UserRepository(PeopleRegistryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Domain.Entities.User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<Domain.Entities.User?> GetByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Domain.Entities.User?> GetById(Guid id) => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}
