using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;

namespace Demo.Events
{
    public abstract class Event<E, D> : IEvent, INotification where E : IEvent where D : IEventData
    {
        public Event()
        {
            Type = GetType().FullName;
            CreatedOn = DateTime.UtcNow;
        }

        [JsonInclude] public D Data { get; protected set; }

        [JsonInclude] public string Type { get; private set; }

        [JsonInclude] public Topics Topic { get; protected set; }

        [JsonInclude] public DateTime CreatedOn { get; private set; }

        [JsonInclude] public string Subject { get; protected set; }

        [JsonInclude] public string DataVersion { get; protected set; }

        [JsonInclude] public Guid CorrelationId { get; protected set; }

        public static E FromJson(string json)
        {
            return JsonSerializer.Deserialize<E>(json, new JsonSerializerOptions());
        }
    }
}