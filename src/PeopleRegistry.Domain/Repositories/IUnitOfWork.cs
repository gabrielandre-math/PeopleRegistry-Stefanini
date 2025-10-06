namespace PeopleRegistry.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
