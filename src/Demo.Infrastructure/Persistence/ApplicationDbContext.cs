using System.Reflection;
using Demo.Domain.ApplicationSettings;
using Demo.Domain.Auditlog;
using Demo.Domain.Customer;
using Demo.Domain.FeatureFlagSettings;
using Demo.Domain.Invoice;
using Demo.Domain.OutboxEvent;
using Demo.Domain.OutboxMessage;
using Demo.Domain.Role;
using Demo.Domain.User;
using Demo.Domain.UserPreferences;
using Demo.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Auditlog> Auditlogs { get; set; }
        public DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<FeatureFlagSettings> FeatureFlagSettings { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }

        // SCAFFOLD-MARKER: DBSET

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Constants.SchemaName);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSequences();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyUtcDateTimeConverter();
        }
    }
}
