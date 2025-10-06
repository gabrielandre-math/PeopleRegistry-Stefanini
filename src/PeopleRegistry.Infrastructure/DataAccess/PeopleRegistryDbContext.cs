using Microsoft.EntityFrameworkCore;
using PeopleRegistry.Domain.Entities;

namespace PeopleRegistry.Infrastructure.DataAccess;

public class PeopleRegistryDbContext : DbContext
{
    public PeopleRegistryDbContext(DbContextOptions<PeopleRegistryDbContext> options) : base(options)
    {
    }
    public DbSet<Person> People { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
}