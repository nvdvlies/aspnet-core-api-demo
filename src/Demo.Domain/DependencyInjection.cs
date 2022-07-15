using System.Linq;
using System.Reflection;
using Demo.Common.Helpers;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.RegisterDomainEntities();
            services.RegisterDomainEntityHooks();

            services.AddScoped<IDomainEntityFactory, DomainEntityFactory>();

            return services;
        }

        private static void RegisterDomainEntities(this IServiceCollection services)
        {
            ClassFinder
                .SearchInAssembly(Assembly.GetExecutingAssembly())
                .ClassesThatImplementInterface(typeof(IDomainEntity<>))
                .ForEach(type =>
                {
                    var nonGenericInterfaceType = type
                        .GetInterfaces()
                        .Where(i => !i.GetTypeInfo().IsGenericType)
                        .FirstOrDefault(i => i.GetInterfaces().Any(j =>
                            j.IsGenericType && j.GetGenericTypeDefinition() == typeof(IDomainEntity<>)));
                    services.AddTransient(nonGenericInterfaceType, type);
                });
        }

        private static void RegisterDomainEntityHooks(this IServiceCollection services)
        {
            var interfaces = new[]
            {
                typeof(IBeforeCreate<>), typeof(IBeforeUpdate<>), typeof(IBeforeDelete<>), typeof(IAfterCreate<>),
                typeof(IAfterUpdate<>), typeof(IAfterDelete<>), typeof(IValidator<>), typeof(IDefaultValuesSetter<>)
            };

            foreach (var @interface in interfaces)
            {
                ClassFinder
                    .SearchInAssembly(Assembly.GetExecutingAssembly())
                    .ClassesThatImplementInterface(@interface)
                    .ForEach(type =>
                    {
                        if (type.IsGenericTypeDefinition) // like LoggingHook<T>
                        {
                            services.AddTransient(@interface, type);
                        }
                        else
                        {
                            var interfaceType = type
                                .GetInterfaces()
                                .Where(i => i.GetTypeInfo().IsGenericType)
                                .FirstOrDefault(i => i.GetGenericTypeDefinition() == @interface);
                            services.AddTransient(interfaceType, type);
                        }
                    });
            }
        }
    }
}
