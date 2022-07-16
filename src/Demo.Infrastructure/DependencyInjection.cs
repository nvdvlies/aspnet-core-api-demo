using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Auth0.ManagementApi;
using Demo.Application.Shared.Interfaces;
using Demo.Common.Extensions;
using Demo.Common.Helpers;
using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Infrastructure.Auditlogging;
using Demo.Infrastructure.Auth0;
using Demo.Infrastructure.Email;
using Demo.Infrastructure.Events;
using Demo.Infrastructure.Messages;
using Demo.Infrastructure.Messages.Consumers;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Services;
using Demo.Infrastructure.Settings;
using Demo.Infrastructure.SignalR;
using Demo.Messages;
using GreenPipes;
using GreenPipes.Configurators;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Demo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        EnvironmentSettings environmentSettings)
    {
        services.AddTransient<IDateTime, DateTimeProvider>();
        services.AddScoped(typeof(IJsonService<>), typeof(JsonService<>));
        services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
        services.AddScoped<IApplicationSettingsProvider, ApplicationSettingsProvider>();
        services.AddScoped<IUserPreferencesProvider, UserPreferencesProvider>();
        services.AddScoped<IFeatureFlagSettingsProvider, FeatureFlagSettingsProvider>();
        services.AddScoped<IFeatureFlagChecker, FeatureFlagChecker>();
        services.AddScoped<IUserIdProvider, UserIdProvider>();
        services.AddScoped<IExternalUserIdProvider, ExternalUserIdProvider>();
        services.AddScoped<ICurrentUserIdProvider, CurrentUserIdProvider>();
        services.AddScoped<IPermissionChecker, PermissionChecker>();
        services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
        services.AddScoped<ICultureProvider, CultureProvider>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IRolesProvider, RolesProvider>();
        services.AddScoped<IPermissionsProvider, PermissionsProvider>();
        services.AddScoped<IPermissionGroupsProvider, PermissionGroupsProvider>();
        services.AddScoped<IUserPermissionsProvider, UserPermissionsProvider>();
        services.AddScoped<IOutboxEventCreator, OutboxEventCreator>();
        services.AddScoped<IOutboxMessageCreator, OutboxMessageCreator>();
        services.AddScoped<IOutboxEventCreatedEvents, OutboxEventCreatedEvents>();
        services.AddScoped<IOutboxMessageCreatedEvents, OutboxMessageCreatedEvents>();
        services.AddScoped<IOutboxEventPublisher, OutboxEventPublisher>();
        services.AddScoped<IOutboxMessageSender, OutboxMessageSender>();
        services.AddSingleton<Microsoft.AspNetCore.SignalR.IUserIdProvider, SignalRUserIdProvider>();
        services.AddScoped<IEmailSender, EmailSender>();

        services.AddMassTransit(s =>
        {
            s.AddConsumers(typeof(RabbitMqEventConsumer).Assembly);

            s.UsingRabbitMq((context, config) =>
            {
                config.ConfigureJsonSerializer(options =>
                {
                    // this is applied globally to (Newtonsoft.Json).SerializerSettings!
                    options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.Culture = CultureInfo.InvariantCulture;
                    options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.Converters.Add(new StringEnumConverter());

                    return options;
                });

                config.Host(new Uri($"rabbitmq://{environmentSettings.RabbitMq.Host}"), h =>
                {
                    h.Username(environmentSettings.RabbitMq.Username);
                    h.Password(environmentSettings.RabbitMq.Password);
                });

                config.ReceiveEndpoint("demo-events", e =>
                {
                    e.Durable = false;
                    e.QueueExpiration = TimeSpan.FromMinutes(15);
                    e.ConfigureConsumer<RabbitMqEventConsumer>(context);
                });

                // 15 retries, first retry after a minute. Subsequent retries after 2,3,4,5 to 15 minutes. Total retry period of 120 minutes.
                void DefaultRetryPolicy(IRetryConfigurator x)
                {
                    x.Incremental(15, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
                }

                config.ReceiveEndpoint(Queues.SynchronizeInvoicePdf.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<SynchronizeInvoicePdfMessageConsumer>(context);
                });

                config.ReceiveEndpoint(Queues.CreateAuth0User.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<CreateAuth0UserMessageConsumer>(context);
                });

                config.ReceiveEndpoint(Queues.SyncAuth0User.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<SyncAuth0UserMessageConsumer>(context);
                });

                config.ReceiveEndpoint(Queues.DeleteAuth0User.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<DeleteAuth0UserMessageConsumer>(context);
                });

                config.ReceiveEndpoint(Queues.SendUserInvitationEmail.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<SendUserInvitationEmailMessageConsumer>(context);
                });

                config.ReceiveEndpoint(Queues.SendChangePasswordEmail.ToString().PascalToKebabCase(), e =>
                {
                    e.Durable = true;
                    e.UseRetry(DefaultRetryPolicy);
                    e.Consumer<SendChangePasswordEmailMessageConsumer>(context);
                });
            });
        });
        services.AddMassTransitHostedService();

        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
        services.AddSingleton<IMessageSender, RabbitMqMessageSender>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(environmentSettings.ConnectionStrings.PostgresDatabase)
        );

        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

        services.AddTransient(typeof(IDbCommand<>), typeof(DbCommand<>));
        services.AddTransient(typeof(IDbCommandForTableWithSingleRecord<>),
            typeof(DbCommandForTableWithSingleRecord<>));

        services.AddDbQueryForEntitiesInDomainAssembly();
        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

        services.AddAuditlogging();

        if (string.IsNullOrEmpty(environmentSettings.Redis.Connection))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = environmentSettings.Redis.Connection;
            });
        }

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
                var interfaceType = typeof(IDbQuery<>).MakeGenericType(type);
                var implementationType = typeof(SoftDeleteDbQuery<>).MakeGenericType(type);
                services.AddTransient(interfaceType, implementationType);
            });

        // Inject DbQuery for entities that do not implement softdelete interface
        ClassFinder
            .SearchInAssembly(domainAssembly)
            .ClassesThatImplementInterface(typeof(IQueryableEntity))
            .ClassesThatDoNotImplementInterface(typeof(ISoftDeleteEntity))
            .ForEach(type =>
            {
                var interfaceType = typeof(IDbQuery<>).MakeGenericType(type);
                var implementationType = typeof(DbQuery<>).MakeGenericType(type);
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
                var interfaceType = type
                    .GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType)
                    .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IAuditlogger<>));

                services.AddTransient(interfaceType!, type);
            });
    }
}
