using System.Collections.Generic;
using System.Linq;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.FeatureFlagSettings;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;

namespace Demo.Infrastructure.Auditlogging
{
    internal class FeatureFlagSettingsAuditlogger : AuditloggerBase<FeatureFlagSettings>,
        IAuditlogger<FeatureFlagSettings>
    {
        public FeatureFlagSettingsAuditlogger(
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUserIdProvider, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(FeatureFlagSettings current, FeatureFlagSettings previous)
        {
            return new AuditlogBuilder<FeatureFlagSettings>()
                .WithChildEntityCollection(c => c.Settings.FeatureFlags, c => c.Name, new AuditlogBuilder<FeatureFlag>()
                    .WithProperty(c => c.Description)
                    .WithProperty(c => c.EnabledForAll)
                    .WithProperty(c => c.EnabledForUsers)
                )
                .Build(current, previous);
        }
    }
}
