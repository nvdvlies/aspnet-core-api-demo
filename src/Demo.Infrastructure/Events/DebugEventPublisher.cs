using Demo.Application.Shared.Interfaces;
using Demo.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Events
{
    public class DebugEventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DebugEventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var eventGridEvent = @event.ToEventGridEvent();
                var @event2 = eventGridEvent.ToEvent();

                await mediator.Publish(@event2, cancellationToken);
            }
        }
    }
}
