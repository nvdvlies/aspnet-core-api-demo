using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.ApplicationSettings
{
    internal class ApplicationSettingsDomainEntity : DomainEntity<ApplicationSettings>, IApplicationSettingsDomainEntity
    {
        private readonly IDbCommandForTableWithSingleRecord<ApplicationSettings> _dbCommand;

        public ApplicationSettingsDomainEntity(
            ILogger<ApplicationSettingsDomainEntity> logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommandForTableWithSingleRecord<ApplicationSettings> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<ApplicationSettings>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<ApplicationSettings>>> validators,
            Lazy<IEnumerable<IBeforeCreate<ApplicationSettings>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<ApplicationSettings>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<ApplicationSettings>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<ApplicationSettings>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<ApplicationSettings>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<ApplicationSettings>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<ApplicationSettings>> jsonService,
            Lazy<IAuditlogger<ApplicationSettings>> auditlogger
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
            throw new DomainException($"{nameof(ApplicationSettings)} cannot be deleted.");
        }
    }
}
