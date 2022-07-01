using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Events
{
    internal class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IBus _bus;
        private readonly ILogger<RabbitMqEventPublisher> _logger;

        public RabbitMqEventPublisher(
            ILogger<RabbitMqEventPublisher> logger,
            IBus bus
        )
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing event with type '{type}'", @event.Type);
            await _bus.Publish(@event.ToRabbitMqEvent(), context => context.CorrelationId = @event.CorrelationId,
                cancellationToken);
        }
    }
}