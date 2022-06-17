using System;
using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.OutboxMessage
{
    public class OutboxMessage : Entity, IQueryableEntity
    {
        [JsonInclude] public string Type { get; internal set; }

        [JsonInclude] public string Message { get; internal set; }

        [JsonInclude] public DateTime? LockedUntil { get; internal set; }

        [JsonInclude] public string LockToken { get; internal set; }

        [JsonInclude] public bool IsSent { get; internal set; }
    }
}