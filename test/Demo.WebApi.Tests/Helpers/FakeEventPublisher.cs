using Demo.Application.Shared.Interfaces;
using Demo.Events;
using Demo.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeEventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public FakeEventPublisher(IServiceProvider serviceProvider)
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
