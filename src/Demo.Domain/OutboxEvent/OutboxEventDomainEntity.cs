using Demo.Common.Interfaces;
using Demo.Domain.OutboxEvent.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace Demo.Domain.OutboxEvent
{
    internal class OutboxEventDomainEntity : DomainEntity<OutboxEvent>, IOutboxEventDomainEntity
    {
        public OutboxEventDomainEntity(
            ILogger<OutboxEventDomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<OutboxEvent> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<OutboxEvent>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<OutboxEvent>>> validators,
            Lazy<IEnumerable<IBeforeCreate<OutboxEvent>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<OutboxEvent>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<OutboxEvent>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<OutboxEvent>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<OutboxEvent>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<OutboxEvent>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreatorr,
            Lazy<IJsonService<OutboxEvent>> jsonService
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, outboxEventCreator, outboxMessageCreatorr, jsonService, null)
        {
        }

        public string Type => Context.Entity?.Type ?? default;

        public void SetEvent(IEvent @event)
        {
            Guard.NotNull(@event, nameof(@event), $"Parameter '{nameof(@event)}' cannot be null.");
            Guard.NotNullOrWhiteSpace(@event.Type, nameof(@event.Type), $"Property '{nameof(@event.Type)}' on parameter '{nameof(@event)}' cannot be null.");

            var assembly = typeof(Event<IEvent, IEventData>).Assembly;
            var eventType = assembly.GetType(@event.Type);

            Guard.NotNull(eventType, nameof(eventType), $"Class of type '{@event.Type}' cannot be found in assembly '{assembly.FullName}'");

            Context.Entity.Type = @event.Type;
            Context.Entity.Event = JsonSerializer.Serialize(@event, eventType, new JsonSerializerOptions());
        }

        public IEvent GetEvent()
        {
            Guard.NotNullOrWhiteSpace(Context.Entity.Type, nameof(Context.Entity.Type), $"{nameof(Context.Entity.Type)} is undefined. Call {nameof(GetAsync)} or {nameof(SetEvent)} first.");
            Guard.NotNullOrWhiteSpace(Context.Entity.Event, nameof(Context.Entity.Event), $"{nameof(Context.Entity.Event)} is undefined. Call {nameof(GetAsync)} or {nameof(SetEvent)} first.");

            var assembly = typeof(Event<IEvent, IEventData>).Assembly;
            var eventType = assembly.GetType(Context.Entity.Type);

            Guard.NotNull(eventType, nameof(eventType), $"Class of type '{Context.Entity.Type}' not found in assembly '{assembly.FullName}'");

            var methodName = nameof(Event<IEvent, IEventData>.FromJson);
            var method = eventType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            Guard.NotNull(method, nameof(method), $"Method '{methodName}' not found in classs of type '{Context.Entity.Type}'");

            var @event = method.Invoke(null, new object[] { Context.Entity.Event });

            return (IEvent)@event;
        }

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
