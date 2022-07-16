using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString =
            "Server=localhost;Port=5502;Database=Demo_Migrations;User Id=postgres;Password=postgres;";
        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Demo.Infrastructure"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
