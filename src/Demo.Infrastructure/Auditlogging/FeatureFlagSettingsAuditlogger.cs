using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.FeatureFlagSettings;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Infrastructure.Auditlogging
{
    internal class FeatureFlagSettingsAuditlogger : AuditloggerBase<FeatureFlagSettings>, IAuditlogger<FeatureFlagSettings>
    {
        public FeatureFlagSettingsAuditlogger(
            ICurrentUser currentUser,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(FeatureFlagSettings current, FeatureFlagSettings previous) =>
            new AuditlogBuilder<FeatureFlagSettings>()
                .WithProperty(c => c.FeatureFlags.Select(x => x.Name).ToList(), nameof(FeatureFlagSettings.FeatureFlags))
                .WithChildEntityCollection(c => c.FeatureFlags, c => c.Name, new AuditlogBuilder<FeatureFlag>()
                    .WithProperty(c => c.EnabledForAll)
                    .WithProperty(c => c.EnabledForUsers)
                )
                .Build(current, previous);
    }
}