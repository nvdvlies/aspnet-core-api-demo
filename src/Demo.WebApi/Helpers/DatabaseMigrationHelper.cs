using Demo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.WebApi.Helpers
{
    public static class DatabaseMigrationHelper
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DatabaseMigrationHelper));
                    try
                    {
                        logger.LogInformation("Starting database migration");
                        appContext.Database.Migrate();
                        logger.LogInformation("Finished database migration");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Database migration failed with error: {0}", ex.Message);
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
