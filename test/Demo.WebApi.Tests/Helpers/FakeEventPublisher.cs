using Demo.Application.Events.Commands.ProcessIncomingEvents;
using Demo.Events;
using Demo.Infrastructure.Events;
using Demo.WebApi.Extensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeEventPublisher : IEventPublisher
    {
        private readonly IMediator _mediator;

        public FakeEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            // transform IEvent to EventGridEvent and back because this is also done in actual implementation path
            var eventGridEvent = @event.ToEventGridEvent();
            var @event2 = eventGridEvent.ToEvent();
            var command = new ProcessIncomingEventsCommand { Events = new[] { @event2 } };
            var handler = new ProcessIncomingEventsCommandHandler(_mediator);

            await handler.Handle(command, cancellationToken);
        }
    }
}
