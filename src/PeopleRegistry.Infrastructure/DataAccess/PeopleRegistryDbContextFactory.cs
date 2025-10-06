using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PeopleRegistry.Infrastructure.DataAccess;


public class PeopleRegistryDbContextFactory : IDesignTimeDbContextFactory<PeopleRegistryDbContext>
{
    public PeopleRegistryDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../PeopleRegistry.Api"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<PeopleRegistryDbContext>();

        optionsBuilder.UseSqlite(connectionString);

        return new PeopleRegistryDbContext(optionsBuilder.Options);
    }
}