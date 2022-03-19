using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxMessage;
using System.Collections.Generic;

namespace Demo.Infrastructure.Messages
{
    internal class OutboxMessageCreatedEvents : List<OutboxMessageCreatedEvent>, IOutboxMessageCreatedEvents
    {
    }
}
