using System.Collections.Generic;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxMessage;

namespace Demo.Infrastructure.Messages;

internal class OutboxMessageCreatedEvents : List<OutboxMessageCreatedEvent>, IOutboxMessageCreatedEvents
{
}
