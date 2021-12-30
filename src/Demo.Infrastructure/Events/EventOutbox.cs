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

        public string Lock(int lockDurationInMinutes = 3)
        {
            if (LockedUntil > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Entity is already locked.");
            }

            var lockToken = Guid.NewGuid().ToString();
            Lock(lockToken, lockDurationInMinutes);
            return lockToken;
        }

        public void Lock(string lockToken, int lockDurationInMinutes = 3)
        {
            LockToken = lockToken;
            LockedUntil = DateTime.UtcNow.AddMinutes(lockDurationInMinutes);
        }

        public void Unlock(string lockToken)
        {
            if (string.Equals(lockToken, LockToken, StringComparison.InvariantCulture))
            {
                LockToken = null;
                LockedUntil = null;
            }
        }

        public void MarkAsPublished()
        {
            IsPublished = true;
        }
    }
}
