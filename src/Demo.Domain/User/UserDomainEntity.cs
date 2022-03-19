using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.User
{
    internal class UserDomainEntity : DomainEntity<User>, IUserDomainEntity
    {
        public UserDomainEntity(
            ILogger<UserDomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<User> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<User>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<User>>> validators,
            Lazy<IEnumerable<IBeforeCreate<User>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<User>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<User>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<User>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<User>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<User>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<User>> jsonService,
            Lazy<IAuditlogger<User>> auditlogger
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, outboxEventCreator, outboxMessageCreator, jsonService, auditlogger)
        {
        }

        internal override Func<IQueryable<User>, IIncludableQueryable<User, object>> Includes => _ => _
            .Include(user => user.UserRoles);
    }
}