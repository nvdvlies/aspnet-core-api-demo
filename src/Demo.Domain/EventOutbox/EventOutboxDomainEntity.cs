using Demo.Common.Interfaces;
using Demo.Domain.EventOutbox.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Demo.Domain.EventOutbox
{
    internal class EventOutboxDomainEntity : DomainEntity<EventOutbox>, IEventOutboxDomainEntity
    {
        public EventOutboxDomainEntity(
            ILogger<EventOutboxDomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<EventOutbox> dbCommand,
            IEnumerable<IDefaultValuesSetter<EventOutbox>> defaultValuesSetters,
            IEnumerable<IValidator<EventOutbox>> validators,
            IEnumerable<IBeforeCreate<EventOutbox>> beforeCreateHooks,
            IEnumerable<IAfterCreate<EventOutbox>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<EventOutbox>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<EventOutbox>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<EventOutbox>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<EventOutbox>> afterDeleteHooks,
            IEventOutboxProcessor eventOutboxProcessor,
            IMessageOutboxProcessor messageOutboxProcessor,
            IJsonService<EventOutbox> jsonService
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, eventOutboxProcessor, messageOutboxProcessor, jsonService, null)
        {
        }

        public string Type => Context.Entity?.Type ?? default;
        public Event Event => Context.Entity?.Event ?? default;

        public void Lock(int lockDurationInMinutes = 3)
        {
            if (Context.Entity.LockedUntil.HasValue && Context.Entity.LockedUntil > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Entity is already locked.");
            }

            Context.Entity.LockToken = Guid.NewGuid().ToString();
            Context.Entity.LockedUntil = DateTime.UtcNow.AddMinutes(lockDurationInMinutes);
        }

        public void Unlock()
        {
            Context.Entity.LockToken = null;
            Context.Entity.LockedUntil = null;
        }

        public void MarkAsPublished()
        {
            Context.Entity.IsPublished = true;
        }
    }
}
