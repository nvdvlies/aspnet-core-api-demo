using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.BusinessComponent.Interfaces;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.BusinessComponent
{
    internal class ApplicationSettingsBusinessComponent : BusinessComponent<ApplicationSettings>, IApplicationSettingsBusinessComponent
    {
        private readonly IDbCommandForTableWithSingleRecord<ApplicationSettings> _dbCommand;

        public ApplicationSettingsBusinessComponent(
            ILogger<ApplicationSettingsBusinessComponent> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommandForTableWithSingleRecord<ApplicationSettings> dbCommand,
            IEnumerable<IDefaultValuesSetter<ApplicationSettings>> defaultValuesSetters,
            IEnumerable<IValidator<ApplicationSettings>> validators,
            IEnumerable<IBeforeCreate<ApplicationSettings>> beforeCreateHooks,
            IEnumerable<IAfterCreate<ApplicationSettings>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<ApplicationSettings>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<ApplicationSettings>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<ApplicationSettings>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<ApplicationSettings>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<ApplicationSettings> jsonService,
            IAuditlog<ApplicationSettings> auditlog
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, publishDomainEventAfterCommitQueue, jsonService, auditlog)
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
