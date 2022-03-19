using Demo.Events.OutboxMessage;
using System.Collections.Generic;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxMessageCreatedEvents : IList<OutboxMessageCreatedEvent>
    {
    }
}
