using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Events
{
    public abstract class Event<E, D> : IEvent where E : IEvent where D : IEventData
    {
        protected Event()
        {
            Type = GetType().FullName;
            CreatedOn = DateTime.UtcNow;
        }

        [JsonInclude] public D Data { get; protected set; }

        [JsonInclude] public string Type { get; private set; }

        [JsonInclude] public Topics Topic { get; protected set; }

        [JsonInclude] public Guid CreatedBy { get; protected set; }

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
