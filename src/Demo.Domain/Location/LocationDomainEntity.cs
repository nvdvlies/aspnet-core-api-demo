using System;
using System.Collections.Generic;
using Demo.Common.Interfaces;
using Demo.Domain.Location.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Location;

internal class LocationDomainEntity : DomainEntity<Location>, ILocationDomainEntity
{
    public LocationDomainEntity(
        ILogger<LocationDomainEntity> logger,
        ICurrentUserIdProvider currentUserIdProvider,
        IDateTime dateTime,
        IDbCommand<Location> dbCommand,
        Lazy<IEnumerable<IDefaultValuesSetter<Location>>> defaultValuesSetters,
        Lazy<IEnumerable<IValidator<Location>>> validators,
        Lazy<IEnumerable<IBeforeCreate<Location>>> beforeCreateHooks,
        Lazy<IEnumerable<IAfterCreate<Location>>> afterCreateHooks,
        Lazy<IEnumerable<IBeforeUpdate<Location>>> beforeUpdateHooks,
        Lazy<IEnumerable<IAfterUpdate<Location>>> afterUpdateHooks,
        Lazy<IEnumerable<IBeforeDelete<Location>>> beforeDeleteHooks,
        Lazy<IEnumerable<IAfterDelete<Location>>> afterDeleteHooks,
        Lazy<IOutboxEventCreator> outboxEventCreator,
        Lazy<IOutboxMessageCreator> outboxMessageCreator,
        Lazy<IJsonService<Location>> jsonService
    )
        : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks,
            afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
            outboxEventCreator, outboxMessageCreator, jsonService, null)
    {
    }
}
