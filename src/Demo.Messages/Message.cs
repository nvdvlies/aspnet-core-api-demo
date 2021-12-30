using System;
using System.Text.Json.Serialization;

namespace Demo.Messages
{
    public class Message<T> : Message where T : IMessageData
    {
        public new T Data => (T)base.Data;

        [JsonConstructor]
        public Message(Queues queue, T data, string subject, string dataVersion, string correlationId) : base(queue, data, subject, dataVersion, correlationId)
        {
        }
    }

    public class Message
    {
        [JsonInclude]
        public string Type { get; private set; }
        [JsonInclude]
        public Queues Queue { get; private set; }
        [JsonInclude]
        public IMessageData Data { get; private set; }
        [JsonInclude]
        public DateTime CreatedOn { get; private set; }
        [JsonInclude]
        public string Subject { get; private set; }
        [JsonInclude]
        public string DataVersion { get; private set; }
        [JsonInclude]
        public string CorrelationId { get; private set; }

        [JsonConstructor]
        public Message(Queues queue, IMessageData data, string subject, string dataVersion, string correlationId)
        {
            Type = GetType().FullName;
            Queue = queue;
            Data = data;
            CreatedOn = DateTime.UtcNow;
            Subject = subject;
            DataVersion = dataVersion;
            CorrelationId = correlationId;
        }
    }
}
