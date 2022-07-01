using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.FeatureFlagSettings
{
    internal class FeatureFlagSettingsDomainEntity : DomainEntity<FeatureFlagSettings>, IFeatureFlagSettingsDomainEntity
    {
        private readonly IDbCommandForTableWithSingleRecord<FeatureFlagSettings> _dbCommand;

        public FeatureFlagSettingsDomainEntity(
            ILogger<FeatureFlagSettingsDomainEntity> logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommandForTableWithSingleRecord<FeatureFlagSettings> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<FeatureFlagSettings>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<FeatureFlagSettings>>> validators,
            Lazy<IEnumerable<IBeforeCreate<FeatureFlagSettings>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<FeatureFlagSettings>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<FeatureFlagSettings>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<FeatureFlagSettings>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<FeatureFlagSettings>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<FeatureFlagSettings>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<FeatureFlagSettings>> jsonService,
            Lazy<IAuditlogger<FeatureFlagSettings>> auditlogger
        )
            : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators,
                beforeCreateHooks,
                afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
                outboxEventCreator, outboxMessageCreator, jsonService, auditlogger)
        {
            _dbCommand = dbCommand;
        }

        public async Task GetAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _dbCommand.GetAsync(cancellationToken);
            if (entity == null)
            {
                await NewAsync(cancellationToken);
                return;
            }

            Context.Entity = entity;
        }

        public override Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            throw new DomainException($"{nameof(FeatureFlagSettings)} cannot be deleted.");
        }
    }
}