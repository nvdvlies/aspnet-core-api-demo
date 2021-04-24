using Demo.Common.Interfaces;
using Demo.Domain.Auditlog.BusinessComponent.Interfaces;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Auditlog.BusinessComponent
{
    internal class AuditlogBusinessComponent : BusinessComponent<Auditlog>, IAuditlogBusinessComponent
    {
        public AuditlogBusinessComponent(
            ILogger<AuditlogBusinessComponent> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<Auditlog> dbCommand,
            IEnumerable<IDefaultValuesSetter<Auditlog>> defaultValuesSetters,
            IEnumerable<IValidator<Auditlog>> validators,
            IEnumerable<IBeforeCreate<Auditlog>> beforeCreateHooks,
            IEnumerable<IAfterCreate<Auditlog>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<Auditlog>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<Auditlog>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<Auditlog>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<Auditlog>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<Auditlog> jsonService
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, publishDomainEventAfterCommitQueue, jsonService, null)
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
}
