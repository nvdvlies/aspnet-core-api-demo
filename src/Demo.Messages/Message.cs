using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;

namespace Demo.Messages;

public abstract class Message<M, D> : IRequest<Unit>, IMessage where M : IMessage where D : IMessageData
{
    protected Message()
    {
        Type = GetType().FullName;
        CreatedOn = DateTime.UtcNow;
    }

    [JsonInclude] public D Data { get; protected set; }

    [JsonInclude] public string Type { get; private set; }

    [JsonInclude] public Queues Queue { get; protected set; }

    [JsonInclude] public Guid CreatedBy { get; protected set; }

    [JsonInclude] public DateTime CreatedOn { get; private set; }

    [JsonInclude] public string Subject { get; protected set; }

    [JsonInclude] public string DataVersion { get; protected set; }

    [JsonInclude] public Guid CorrelationId { get; protected set; }

    public static M FromJson(string json)
    {
        return JsonSerializer.Deserialize<M>(json, new JsonSerializerOptions());
    }
}
