using System.Reflection;
using System.Text.Json;
using Demo.Events;

namespace Demo.Infrastructure.Events
{
    internal static class Extensions
    {
        public static RabbitMqEvent ToRabbitMqEvent(this IEvent @event)
        {
            var assembly = typeof(Event<IEvent, IEventData>).Assembly;
            var eventType = assembly.GetType(@event.Type);
            var payload = JsonSerializer.Serialize(@event, eventType, new JsonSerializerOptions());
            return new RabbitMqEvent { ContentType = @event.Type, Payload = payload };
        }

        public static IEvent ToEvent(this RabbitMqEvent rabbitMqEvent)
        {
            var eventType = typeof(Event<IEvent, IEventData>).Assembly.GetType(rabbitMqEvent.ContentType);
            var methodName = nameof(Event<IEvent, IEventData>.FromJson);
            var method = eventType.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var @event = method.Invoke(null, new object[] { rabbitMqEvent.Payload });
            return (IEvent)@event;
        }
    }
}
