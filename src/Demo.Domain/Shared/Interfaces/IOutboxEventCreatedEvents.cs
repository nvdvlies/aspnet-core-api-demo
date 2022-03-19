using Demo.Events.OutboxEvent;
using System.Collections.Generic;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxEventCreatedEvents : IList<OutboxEventCreatedEvent>
    {
    }
}
