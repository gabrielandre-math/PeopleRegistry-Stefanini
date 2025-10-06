using PeopleRegistry.Domain.Entities;

namespace PeopleRegistry.Domain.Repositories.Users;  

public interface IUserRepository
{
    Task Add(User user);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(Guid id);
}