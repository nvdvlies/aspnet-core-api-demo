using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Demo.Common.Interfaces;
using Demo.Domain.OutboxMessage.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.OutboxMessage
{
    internal class OutboxMessageDomainEntity : DomainEntity<OutboxMessage>, IOutboxMessageDomainEntity
    {
        public OutboxMessageDomainEntity(
            ILogger<OutboxMessageDomainEntity> logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommand<OutboxMessage> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<OutboxMessage>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<OutboxMessage>>> validators,
            Lazy<IEnumerable<IBeforeCreate<OutboxMessage>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<OutboxMessage>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<OutboxMessage>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<OutboxMessage>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<OutboxMessage>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<OutboxMessage>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<OutboxMessage>> jsonService
        )
            : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators,
                beforeCreateHooks,
                afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
                outboxEventCreator, outboxMessageCreator, jsonService, null)
        {
        }

        public string Type => Context.Entity?.Type ?? default;

        public void SetMessage(IMessage message)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(NewAsync)} first to instantiate entity");
            Guard.NotNull(message, nameof(message), $"Parameter '{nameof(message)}' cannot be null.");
            Guard.NotNullOrWhiteSpace(message.Type, nameof(message.Type),
                $"Property '{nameof(message.Type)}' on parameter '{nameof(message)}' cannot be null.");

            var assembly = typeof(Message<IMessage, IMessageData>).Assembly;
            var messageType = assembly.GetType(message.Type);

            Guard.NotNull(messageType, nameof(messageType),
                $"Class of type '{message.Type}' cannot be found in assembly '{assembly.FullName}'");

            Context.Entity.Type = message.Type;
            Context.Entity.Message = JsonSerializer.Serialize(message, messageType, new JsonSerializerOptions());
        }

        public IMessage GetMessage()
        {
            Guard.NotNullOrWhiteSpace(Context.Entity.Type, nameof(Context.Entity.Type),
                $"{nameof(Context.Entity.Type)} is undefined. Call {nameof(GetAsync)} or {nameof(SetMessage)} first.");
            Guard.NotNullOrWhiteSpace(Context.Entity.Message, nameof(Context.Entity.Message),
                $"{nameof(Context.Entity.Message)} is undefined. Call {nameof(GetAsync)} or {nameof(SetMessage)} first.");

            var assembly = typeof(Message<IMessage, IMessageData>).Assembly;
            var messageType = assembly.GetType(Context.Entity.Type);

            Guard.NotNull(messageType, nameof(messageType),
                $"Class of type '{Context.Entity.Type}' not found in assembly '{assembly.FullName}'");

            var methodName = nameof(Message<IMessage, IMessageData>.FromJson);
            var method = messageType.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            Guard.NotNull(method, nameof(method),
                $"Method '{methodName}' not found in classs of type '{Context.Entity.Type}'");

            var message = method.Invoke(null, new object[] { Context.Entity.Message });

            return (IMessage)message;
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

        public void MarkAsSent()
        {
            Context.Entity.IsSent = true;
        }
    }
}
