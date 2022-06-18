using System.Collections.Generic;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences;
using Demo.Infrastructure.Auditlogging.Shared;

namespace Demo.Infrastructure.Auditlogging
{
    internal class UserPreferencesAuditlogger : AuditloggerBase<UserPreferences>, IAuditlogger<UserPreferences>
    {
        public UserPreferencesAuditlogger(
            ICurrentUser currentUser,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(UserPreferences current, UserPreferences previous)
        {
            return new AuditlogBuilder<UserPreferences>()
                .WithChildEntity(x => x.Preferences, new AuditlogBuilder<UserPreferencesPreferences>()
                    .WithProperty(c => c.Setting1)
                    .WithProperty(c => c.Setting2)
                    .WithProperty(c => c.Setting3)
                    .WithProperty(c => c.Setting4)
                    .WithProperty(c => c.Setting5)
                )
                .Build(current, previous);
        }
    }
}
