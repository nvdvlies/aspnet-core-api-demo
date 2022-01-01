using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.MessageOutbox
{
    public class MessageOutbox : Entity, IQueryableEntity
    {
        public string Type { get; set; }
        public Message Message { get; set; }
        [JsonInclude]
        public DateTime? LockedUntil { get; internal set; }
        [JsonInclude]
        public string LockToken { get; internal set; }
        [JsonInclude]
        public bool IsSent { get; internal set; }
    }
}
