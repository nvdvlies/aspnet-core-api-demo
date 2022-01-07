using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Messages
{
    public abstract class Message<M, D> : IMessage where M : IMessage where D : IMessageData
    {
        [JsonInclude]
        public string Type { get; private set; }
        [JsonInclude]
        public Queues Queue { get; protected set; }
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

        public Message()
        {
            Type = GetType().FullName;
            CreatedOn = DateTime.UtcNow;
        }

        public static M FromJson(string json)
        {
            return JsonSerializer.Deserialize<M>(json, new JsonSerializerOptions());
        }
    }
}
