using Demo.Application.Shared.Interfaces;
using Demo.Common.Helpers;
using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.QueueOutbox;
using Demo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Demo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddScoped(typeof(IJsonService<>), typeof(JsonService<>));
            services.AddScoped<IApplicationSettingsProvider, ApplicationSettingsProvider>();
            services.AddScoped<IPublishEventAfterCommitQueue, PublishEventAfterCommitQueue>();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SqlDatabase"))
                );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient(typeof(IDbCommand<>), typeof(DbCommand<>));
            services.AddTransient(typeof(IDbCommandForTableWithSingleRecord<>), typeof(DbCommandForTableWithSingleRecord<>));

            services.AddDbQueryForEntitiesInDomainAssembly();
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddAuditlogging();

            services.AddMemoryCache();

            return services;
        }

        private static void AddDbQueryForEntitiesInDomainAssembly(this IServiceCollection services)
        {
            var domainAssembly = typeof(Entity).Assembly;

            // Inject SoftDeleteDbQuery for entities that implement softdelete interface
            ClassFinder
                .SearchInAssembly(domainAssembly)
                .ClassesThatImplementInterface(typeof(IQueryableEntity))
                .ClassesThatImplementInterface(typeof(ISoftDeleteEntity))
                .ForEach(type =>
                {
                    var interfaceType = typeof(IDbQuery<>).MakeGenericType(new[] { type });
                    var implementationType = typeof(SoftDeleteDbQuery<>).MakeGenericType(new[] { type });
                    services.AddTransient(interfaceType, implementationType);
                });

            // Inject DbQuery for entities that do not implement softdelete interface
            ClassFinder
                .SearchInAssembly(domainAssembly)
                .ClassesThatImplementInterface(typeof(IQueryableEntity))
                .ClassesThatDoNotImplementInterface(typeof(ISoftDeleteEntity))
                .ForEach(type =>
                {
                    var interfaceType = typeof(IDbQuery<>).MakeGenericType(new[] { type });
                    var implementationType = typeof(Persistence.DbQuery<>).MakeGenericType(new[] { type });
                    services.AddTransient(interfaceType, implementationType);
                });
        }

        private static void AddAuditlogging(this IServiceCollection services)
        {
            var auditloggersAssembly = typeof(InvoiceAuditlogger).Assembly;
            var assembliesToScan = new[] { auditloggersAssembly };

            ClassFinder
                .SearchInAssemblies(assembliesToScan)
                .ClassesThatImplementInterface(typeof(IAuditlogger<>))
                .ForEach(type =>
                {
                    var interfaceType = type.GetInterfaces()
                        .Where(i => i.GetTypeInfo().IsGenericType)
                        .Where(i => i.GetGenericTypeDefinition() == typeof(IAuditlogger<>))
                        .FirstOrDefault();
                    services.AddTransient(interfaceType, type);
                });
        }
    }
}
