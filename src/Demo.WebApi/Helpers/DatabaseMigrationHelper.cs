using System;
using System.Linq;
using Demo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.WebApi.Helpers
{
    public static class DatabaseMigrationHelper
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger(nameof(DatabaseMigrationHelper));
            try
            {
                logger.LogInformation("Retrieving pending database migrations.");

                var pendingMigrations = appContext.Database.GetPendingMigrations().ToList();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Pending database migrations are available.");

                    var lastAppliedMigration = appContext.Database.GetAppliedMigrations().LastOrDefault();
                    if (!string.IsNullOrEmpty(lastAppliedMigration))
                    {
                        logger.LogInformation("Database currently on migration '{lastAppliedMigration}'.",
                            lastAppliedMigration);
                    }

                    logger.LogInformation("Applying {count} migration(s) ({names}).", pendingMigrations.Count(),
                        string.Join(",", pendingMigrations));

                    appContext.Database.Migrate();

                    lastAppliedMigration = appContext.Database.GetAppliedMigrations().Last();

                    logger.LogInformation("Database migrated to '{lastAppliedMigration}'.", lastAppliedMigration);
                }
                else
                {
                    logger.LogInformation("No pending database migrations available.");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Database migration failed with error: {message}.", ex.Message);
                throw;
            }

            return host;
        }
    }
}
