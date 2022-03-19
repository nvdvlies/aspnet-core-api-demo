using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using System.Collections.Generic;

namespace Demo.Infrastructure.Events
{
    internal class OutboxEventCreatedEvents : List<OutboxEventCreatedEvent>, IOutboxEventCreatedEvents
    {
    }
}
