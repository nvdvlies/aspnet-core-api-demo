using Azure.Messaging.EventGrid;
using Demo.Events;
using MediatR;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Events.Commands.ProcessIncomingEvents
{
    public class ProcessIncomingEventsCommandHandler : IRequestHandler<ProcessIncomingEventsCommand, Unit>
    {
        private readonly IMediator _mediator;

        public ProcessIncomingEventsCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessIncomingEventsCommand request, CancellationToken cancellationToken)
        {
            foreach (var eventGridEvent in request.EventGridEvents)
            {
                var @event = eventGridEvent.ToEvent();
                await _mediator.Publish(@event, cancellationToken);
            }

            return Unit.Value;
        }
    }

    public static class Extensions
    {
        public static object ToEvent(this EventGridEvent eventGridEvent)
        {
            var eventType = typeof(Event).Assembly.GetType(eventGridEvent.EventType);
            var methodName = nameof(Event<object, IEventData>.ToObjectFromJson);
            var method = eventType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var @event = method.Invoke(null, new object[] { eventGridEvent.Data });
            return @event;
        }
    }
}