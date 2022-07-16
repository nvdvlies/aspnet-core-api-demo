using System.Collections.Generic;
using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;

namespace Demo.Infrastructure.Auditlogging;

internal class ApplicationSettingsAuditlogger : AuditloggerBase<ApplicationSettings>,
    IAuditlogger<ApplicationSettings>
{
    public ApplicationSettingsAuditlogger(
        ICurrentUserIdProvider currentUserIdProvider,
        IDateTime dateTime,
        IAuditlogDomainEntity auditlogDomainEntity
    ) : base(currentUserIdProvider, dateTime, auditlogDomainEntity)
    {
    }

    protected override List<AuditlogItem> AuditlogItems(ApplicationSettings current, ApplicationSettings previous)
    {
        return new AuditlogBuilder<ApplicationSettings>()
            .WithChildEntity(x => x.Settings, new AuditlogBuilder<ApplicationSettingsSettings>()
                .WithProperty(c => c.Setting1)
                .WithProperty(c => c.Setting2)
                .WithProperty(c => c.Setting3, AuditlogType.DateOnly)
                .WithProperty(c => c.Setting4)
                .WithProperty(c => c.Setting5)
            )
            .Build(current, previous);
    }
}
