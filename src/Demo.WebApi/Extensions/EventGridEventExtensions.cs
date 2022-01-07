using Azure.Messaging.EventGrid;
using Demo.Events;
using System.Reflection;

namespace Demo.WebApi.Extensions
{
    public static class EventGridEventExtensions
    {
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