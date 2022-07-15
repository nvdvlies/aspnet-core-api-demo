using System.Collections.Generic;
using Demo.Events.OutboxEvent;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxEventCreatedEvents : IList<OutboxEventCreatedEvent>
    {
    }
}
