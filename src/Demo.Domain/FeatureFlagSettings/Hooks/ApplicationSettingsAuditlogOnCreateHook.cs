using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.FeatureFlagSettings.Hooks
{
    internal class FeatureFlagSettingsAuditlogOnCreateHook : IAfterCreate<FeatureFlagSettings>
    {
        private readonly Lazy<IAuditlogger<FeatureFlagSettings>> _auditlogger;

        public FeatureFlagSettingsAuditlogOnCreateHook(
            Lazy<IAuditlogger<FeatureFlagSettings>> auditlogger
        )
        {
            _auditlogger = auditlogger;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<FeatureFlagSettings> context,
            CancellationToken cancellationToken)
        {
            await _auditlogger.Value.CreateAuditLogAsync(context.Entity, context.Pristine, cancellationToken);
        }
    }
}