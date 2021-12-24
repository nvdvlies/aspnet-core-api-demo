using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;
using System.Collections.Generic;

namespace Demo.Infrastructure.Auditlogging
{
    internal class ApplicationSettingsAuditlogger : AuditlogBase<ApplicationSettings>, IAuditlog<ApplicationSettings>
    {
        public ApplicationSettingsAuditlogger(
            ICurrentUser currentUser, 
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(ApplicationSettings current, ApplicationSettings previous) =>
            new AuditlogBuilder<ApplicationSettings>()
                .WithChildEntity(x => x.Settings, new AuditlogBuilder<ApplicationSettingsSettings>()
                    .WithProperty(c => c.Setting1)
                    .WithProperty(c => c.Setting2)
                    .WithProperty(c => c.Setting3)
                    .WithProperty(c => c.Setting4)
                    .WithProperty(c => c.Setting5)
                )
                .Build(current, previous);
    }
}
