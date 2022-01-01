using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.EventOutbox
{
    public class EventOutbox : Entity, IQueryableEntity
    {
        public string Type { get; set; }
        public Event Event { get; set; }
        [JsonInclude]
        public DateTime? LockedUntil { get; internal set; }
        [JsonInclude]
        public string LockToken { get; internal set; }
        [JsonInclude]
        public bool IsPublished { get; internal set; }
    }
}
