using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Auditlog;

internal class AuditlogDomainEntity : DomainEntity<Auditlog>, IAuditlogDomainEntity
{
    public AuditlogDomainEntity(
        ILogger<AuditlogDomainEntity> logger,
        ICurrentUserIdProvider currentUserIdProvider,
        IDateTime dateTime,
        IDbCommand<Auditlog> dbCommand,
        Lazy<IEnumerable<IDefaultValuesSetter<Auditlog>>> defaultValuesSetters,
        Lazy<IEnumerable<IValidator<Auditlog>>> validators,
        Lazy<IEnumerable<IBeforeCreate<Auditlog>>> beforeCreateHooks,
        Lazy<IEnumerable<IAfterCreate<Auditlog>>> afterCreateHooks,
        Lazy<IEnumerable<IBeforeUpdate<Auditlog>>> beforeUpdateHooks,
        Lazy<IEnumerable<IAfterUpdate<Auditlog>>> afterUpdateHooks,
        Lazy<IEnumerable<IBeforeDelete<Auditlog>>> beforeDeleteHooks,
        Lazy<IEnumerable<IAfterDelete<Auditlog>>> afterDeleteHooks,
        Lazy<IOutboxEventCreator> outboxEventCreator,
        Lazy<IOutboxMessageCreator> outboxMessageCreator,
        Lazy<IJsonService<Auditlog>> jsonService
    )
        : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators,
            beforeCreateHooks,
            afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
            outboxEventCreator, outboxMessageCreator, jsonService, null)
    {
    }

    public override Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        throw new DomainException($"{nameof(Auditlog)} cannot be updated.");
    }

    public override Task UpsertAsync(CancellationToken cancellationToken = default)
    {
        throw new DomainException($"{nameof(Auditlog)} cannot be updated. Use InsertAsync() instead.");
    }
}
