using Demo.Application.Events.Commands.ProcessIncomingEvents;
using Demo.Events;
using Demo.Infrastructure.Events;
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

        public async Task PublishAsync(Event @event, CancellationToken cancellationToken)
        {
            var eventGridEvent = @event.ToEventGridEvent();
            var command = new ProcessIncomingEventsCommand { EventGridEvents = new[] { eventGridEvent } };
            var handler = new ProcessIncomingEventsCommandHandler(_mediator);

            await handler.Handle(command, cancellationToken);
        }
    }
}
