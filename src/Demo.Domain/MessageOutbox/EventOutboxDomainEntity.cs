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
            Lazy<IEnumerable<IDefaultValuesSetter<MessageOutbox>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<MessageOutbox>>> validators,
            Lazy<IEnumerable<IBeforeCreate<MessageOutbox>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<MessageOutbox>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<MessageOutbox>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<MessageOutbox>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<MessageOutbox>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<MessageOutbox>>> afterDeleteHooks,
            Lazy<IEventOutboxProcessor> eventOutboxProcessor,
            Lazy<IMessageOutboxProcessor> messageOutboxProcessor,
            Lazy<IJsonService<MessageOutbox>> jsonService
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, eventOutboxProcessor, messageOutboxProcessor, jsonService, null)
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
