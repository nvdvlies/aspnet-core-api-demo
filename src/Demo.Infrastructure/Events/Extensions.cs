using Azure.Messaging.EventGrid;
using Demo.Events;

namespace Demo.Infrastructure.Events
{
    public static class Extensions
    {
        public static EventGridEvent ToEventGridEvent(this IEvent @event)
        {
            return new EventGridEvent(@event.Subject, @event.Type, @event.DataVersion, @event)
            {
                Topic = @event.Topic.ToString(),
                EventTime = @event.CreatedOn
            };
        }
    }
}
