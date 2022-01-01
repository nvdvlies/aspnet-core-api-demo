using Demo.Common.Interfaces;
using Demo.Domain.MessageOutbox.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Demo.Domain.MessageOutbox
{
    internal class MessageOutboxDomainEntity : DomainEntity<MessageOutbox>, IMessageOutboxDomainEntity
    {
        public MessageOutboxDomainEntity(
            ILogger<MessageOutboxDomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<MessageOutbox> dbCommand,
            IEnumerable<IDefaultValuesSetter<MessageOutbox>> defaultValuesSetters,
            IEnumerable<IValidator<MessageOutbox>> validators,
            IEnumerable<IBeforeCreate<MessageOutbox>> beforeCreateHooks,
            IEnumerable<IAfterCreate<MessageOutbox>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<MessageOutbox>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<MessageOutbox>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<MessageOutbox>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<MessageOutbox>> afterDeleteHooks,
            IEventOutboxProcessor eventOutboxProcessor,
            IMessageOutboxProcessor messageOutboxProcessor,
            IJsonService<MessageOutbox> jsonService,
            IAuditlogger<MessageOutbox> auditlogger
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, eventOutboxProcessor, messageOutboxProcessor, jsonService, auditlogger)
        {
        }

        public string Type => Context.Entity?.Type ?? default;
        public Message Message => Context.Entity?.Message ?? default;

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

        public void MarkAsSent()
        {
            Context.Entity.IsSent = true;
        }
    }
}
