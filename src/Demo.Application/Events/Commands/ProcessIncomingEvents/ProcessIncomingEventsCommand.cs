using Azure.Messaging.EventGrid;
using MediatR;

namespace Demo.Application.Events.Commands.ProcessIncomingEvents
{
    public class ProcessIncomingEventsCommand : IRequest<Unit>
    {
        public EventGridEvent[] EventGridEvents { get; set; }
    }
}