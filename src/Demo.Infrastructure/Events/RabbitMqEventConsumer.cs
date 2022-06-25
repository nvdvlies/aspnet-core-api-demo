using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Demo.Infrastructure.Events
{
    public class RabbitMqEventConsumer : IConsumer<RabbitMqEvent>
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqEventConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<RabbitMqEvent> context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<RabbitMqEventConsumer>>();
                var correlationIdProvider = scope.ServiceProvider.GetRequiredService<ICorrelationIdProvider>();
                var currentUserIdProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserIdProvider>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var @event = context.Message.ToEvent();

                currentUserIdProvider.SetUserId(@event.CreatedBy);

                correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
                using (LogContext.PushProperty("CorrelationId", correlationIdProvider.Id))
                {
                    logger.LogInformation("Consuming {0} with type '{1}'", nameof(RabbitMqEvent),
                        context.Message.ContentType);
                    await mediator.Publish(@event, context.CancellationToken);
                }
            }
        }
    }
}
