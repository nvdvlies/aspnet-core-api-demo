using Azure.Messaging.EventGrid;
using Demo.Events;
using System.Reflection;

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

        public static IEvent ToEvent(this EventGridEvent eventGridEvent)
        {
            var eventType = typeof(Event<IEvent, IEventData>).Assembly.GetType(eventGridEvent.EventType);
            var methodName = nameof(Event<IEvent, IEventData>.FromBinaryData);
            var method = eventType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var @event = method.Invoke(null, new object[] { eventGridEvent.Data });
            return (IEvent)@event;
        }
    }
}
