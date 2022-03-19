using Demo.Application.Invoices.Services;
using Demo.Application.Shared.PipelineBehaviors;
using Demo.Domain.Invoice.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Demo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AssemblyScanner.FindValidatorsInAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .ForEach(pair =>
                {
                    services.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
                    services.Add(ServiceDescriptor.Transient(pair.ValidatorType, pair.ValidatorType));
                });

            services.AddAutoMapper(new[] { Assembly.GetExecutingAssembly() });

            services.AddMediatR(new[] { Assembly.GetExecutingAssembly() });

            services.RegisterPipelineBehaviors();

            services.RegisterServices();

            return services;
        }

        private static void RegisterPipelineBehaviors(this IServiceCollection services)
        {
            // Please be aware that the order in which PipelineBehaviors are executed 
            // is determined by the order in which PipelineBehaviors are registered for
            // dependency injection.  

            // Current configuration:

            // | direction | logging | validation | send messages | publish events | uow | handler |
            // |-----------|---------|------------|---------------|----------------|-----|---------|
            // |   -->     |   1     |     2      |               |                |     |    3    |
            // |   <--     |   7     |            |       6       |       5        |  4  |         |
            // |-----------|---------|------------|---------------|----------------|-----|---------|

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ProcessOutboxMessageCreatedEventsPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ProcessOutboxEventCreatedEventsPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IInvoiceToPdfModelMapper, InvoiceToPdfModelMapper>();
        }
    }
}
