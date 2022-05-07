using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = "Data Source=localhost;initial catalog=Demo;Integrated Security=True; MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Demo.Infrastructure"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
