using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using System;
using System.Text.Json.Serialization;

namespace Demo.Infrastructure.Events
{
    public class EventOutbox : Entity, IQueryableEntity
    {
        public string Type { get; set; }
        public Event Event { get; set; }
        [JsonInclude]
        public DateTime? LockedUntil { get; private set; }
        [JsonInclude]
        public string LockToken { get; private set; }
        [JsonInclude]
        public bool IsPublished { get; private set; }

        public void Lock(int lockDurationInMinutes = 3)
        {
            if (LockedUntil.HasValue && LockedUntil > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Entity is already locked.");
            }

            LockToken = Guid.NewGuid().ToString();
            LockedUntil = DateTime.UtcNow.AddMinutes(lockDurationInMinutes);
        }

        public void Unlock()
        {
            LockToken = null;
            LockedUntil = null;
        }

        public void MarkAsPublished()
        {
            IsPublished = true;
        }
    }
}
