using Auth0.ManagementApi;
using Demo.Application.Shared.Interfaces;
using Demo.Common.Helpers;
using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging;
using Demo.Infrastructure.Auth0;
using Demo.Infrastructure.Events;
using Demo.Infrastructure.Messages;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Services;
using Demo.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Demo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, EnvironmentSettings environmentSettings)
        {
            services.AddTransient<IDateTime, DateTimeProvider>();
            services.AddScoped(typeof(IJsonService<>), typeof(JsonService<>));
            services.AddScoped<IApplicationSettingsProvider, ApplicationSettingsProvider>();
            services.AddScoped<IFeatureFlagSettingsProvider, FeatureFlagSettingsProvider>();
            services.AddScoped<IFeatureFlagChecker, FeatureFlagChecker>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IOutboxEventCreator, OutboxEventCreator>();
            services.AddScoped<IOutboxMessageCreator, OutboxMessageCreator>();
            services.AddScoped<IOutboxEventCreatedEvents, OutboxEventCreatedEvents>();
            services.AddScoped<IOutboxMessageCreatedEvents, OutboxMessageCreatedEvents>();
            services.AddScoped<IOutboxEventPublisher, OutboxEventPublisher>();
            services.AddScoped<IOutboxMessageSender, OutboxMessageSender>();
#if DEBUG
            services.AddSingleton<IEventPublisher, DebugEventPublisher>();
#else
            services.AddSingleton<IEventPublisher, EventGridPublisher>();
#endif
#if DEBUG
            services.AddSingleton<IMessageSender, DebugMessageSender>();
#else
            services.AddSingleton<IMessageSender, ServiceBusQueueMessageSender>();
#endif
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SqlDatabase"))
                );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient(typeof(IDbCommand<>), typeof(DbCommand<>));
            services.AddTransient(typeof(IDbCommandForTableWithSingleRecord<>), typeof(DbCommandForTableWithSingleRecord<>));

            services.AddDbQueryForEntitiesInDomainAssembly();
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddAuditlogging();

#if DEBUG
            services.AddDistributedMemoryCache();
#else
            services.AddStackExchangeRedisCache(options =>  
            {  
                options.Configuration = environmentSettings.Redis.Connection;  
            });  
#endif
            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
            services.AddScoped<IAuth0UserManagementClient, Auth0UserManagementClient>();
            services.AddScoped<IAuth0ManagementApiClientProvider, Auth0ManagementApiClientProvider>();

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
