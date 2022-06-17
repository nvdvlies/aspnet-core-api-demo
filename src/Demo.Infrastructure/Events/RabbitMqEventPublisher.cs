using Demo.Application.Shared.Interfaces;
using Demo.Events;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Demo.Infrastructure.Events
{
    internal class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IBus _bus;

        public RabbitMqEventPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            await _bus.Publish(@event.ToRabbitMqEvent(), context => context.CorrelationId = @event.CorrelationId, cancellationToken);
        }
    }
}
