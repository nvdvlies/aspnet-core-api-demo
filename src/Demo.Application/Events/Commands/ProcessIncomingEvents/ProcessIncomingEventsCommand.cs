using Demo.Events;
using MediatR;
using System.Collections.Generic;

namespace Demo.Application.Events.Commands.ProcessIncomingEvents
{
    public class ProcessIncomingEventsCommand : IRequest<Unit>
    {
        public IEnumerable<INotification> Events { get; set; }
    }
}