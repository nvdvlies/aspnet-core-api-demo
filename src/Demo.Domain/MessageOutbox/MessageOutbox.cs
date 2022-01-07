using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.MessageOutbox
{
    public class MessageOutbox : Entity, IQueryableEntity
    {
        [JsonInclude]
        public string Type { get; internal set; }
        [JsonInclude]
        public string Message { get; internal set; }
        [JsonInclude]
        public DateTime? LockedUntil { get; internal set; }
        [JsonInclude]
        public string LockToken { get; internal set; }
        [JsonInclude]
        public bool IsSent { get; internal set; }
    }
}
