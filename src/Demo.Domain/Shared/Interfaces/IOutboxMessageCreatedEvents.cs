using System.Collections.Generic;
using Demo.Events.OutboxMessage;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxMessageCreatedEvents : IList<OutboxMessageCreatedEvent>
    {
    }
}
