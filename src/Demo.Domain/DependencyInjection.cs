using Demo.Common.Helpers;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

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
                    var nonGenericInterfaceType = type.GetInterfaces()
                        .Where(i => !i.GetTypeInfo().IsGenericType)
                        .Where(i => i.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEntity<>)))
                        .FirstOrDefault();
                    services.AddTransient(nonGenericInterfaceType, type);
                });
        }

        private static void RegisterDomainEntityHooks(this IServiceCollection services)
        {
            var interfaces = new[]
            {
                typeof(IBeforeCreate<>),
                typeof(IBeforeUpdate<>),
                typeof(IBeforeDelete<>),
                typeof(IAfterCreate<>),
                typeof(IAfterUpdate<>),
                typeof(IAfterDelete<>),
                typeof(IValidator<>),
                typeof(IDefaultValuesSetter<>)
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
                            var interfaceType = type.GetInterfaces()
                                .Where(i => i.GetTypeInfo().IsGenericType)
                                .Where(i => i.GetGenericTypeDefinition() == @interface)
                                .FirstOrDefault();
                            services.AddTransient(interfaceType, type);
                        }
                    });
            }
        }
    }
}
