using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.UserPreferences
{
    internal class UserPreferencesDomainEntity : DomainEntity<UserPreferences>, IUserPreferencesDomainEntity
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IDbCommand<UserPreferences> _dbCommand;

        public UserPreferencesDomainEntity(
            ILogger<UserPreferencesDomainEntity> logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommand<UserPreferences> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<UserPreferences>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<UserPreferences>>> validators,
            Lazy<IEnumerable<IBeforeCreate<UserPreferences>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<UserPreferences>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<UserPreferences>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<UserPreferences>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<UserPreferences>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<UserPreferences>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<UserPreferences>> jsonService,
            Lazy<IAuditlogger<UserPreferences>> auditlogger
        )
            : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators,
                beforeCreateHooks,
                afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
                outboxEventCreator, outboxMessageCreator, jsonService, auditlogger)
        {
            _currentUserIdProvider = currentUserIdProvider;
            _dbCommand = dbCommand;
        }

        public async Task GetAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserIdProvider.Id;
            var entity = await _dbCommand.GetAsync(userId, cancellationToken: cancellationToken);
            if (entity == null)
            {
                await NewAsync(cancellationToken);
            }
            else
            {
                Context.Entity = entity;
            }
        }
    }
}
