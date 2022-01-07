using MediatR;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Events
{
    public abstract class Event<E, D> : IEvent, INotification where E : IEvent where D : IEventData
    {
        [JsonInclude]
        public string Type { get; private set; }
        [JsonInclude]
        public Topics Topic { get; protected set; }
        [JsonInclude]
        public D Data { get; protected set; }
        [JsonInclude]
        public DateTime CreatedOn { get; private set; }
        [JsonInclude]
        public string Subject { get; protected set; }
        [JsonInclude]
        public string DataVersion { get; protected set; }
        [JsonInclude]
        public string CorrelationId { get; protected set; }

        public Event()
        {
            Type = GetType().FullName;
            CreatedOn = DateTime.UtcNow;
        }

        public static E FromBinaryData(BinaryData binaryData)
        {
            return binaryData.ToObjectFromJson<E>();
        }

        public static E FromJson(string json)
        {
            return JsonSerializer.Deserialize<E>(json, new JsonSerializerOptions());
        }
    }
}
