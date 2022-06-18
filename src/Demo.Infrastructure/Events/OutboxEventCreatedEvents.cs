using System.Collections.Generic;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;

namespace Demo.Infrastructure.Events
{
    internal class OutboxEventCreatedEvents : List<OutboxEventCreatedEvent>, IOutboxEventCreatedEvents
    {
    }
}
