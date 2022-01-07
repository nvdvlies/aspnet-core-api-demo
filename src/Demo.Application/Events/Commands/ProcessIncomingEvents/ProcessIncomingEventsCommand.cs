using Demo.Events;
using MediatR;
using System.Collections.Generic;

namespace Demo.Application.Events.Commands.ProcessIncomingEvents
{
    public class ProcessIncomingEventsCommand : IRequest<Unit>
    {
        public IEnumerable<IEvent> Events { get; set; }
    }
}