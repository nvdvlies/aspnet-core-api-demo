using System;
using System.Collections.Generic;
using Demo.Common.Interfaces;
using Demo.Domain.Role.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Role
{
    internal class RoleDomainEntity : DomainEntity<Role>, IRoleDomainEntity
    {
        public RoleDomainEntity(
            ILogger<RoleDomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<Role> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<Role>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<Role>>> validators,
            Lazy<IEnumerable<IBeforeCreate<Role>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<Role>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<Role>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<Role>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<Role>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<Role>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<Role>> jsonService,
            Lazy<IAuditlogger<Role>> auditlogger
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks,
                afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
                outboxEventCreator, outboxMessageCreator, jsonService, auditlogger)
        {
        }
    }
}