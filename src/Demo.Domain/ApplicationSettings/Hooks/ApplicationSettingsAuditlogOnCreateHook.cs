using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.ApplicationSettings.Hooks
{
    internal class ApplicationSettingsAuditlogOnCreateHook : IAfterCreate<ApplicationSettings>
    {
        private readonly Lazy<IAuditlogger<ApplicationSettings>> _auditlogger;

        public ApplicationSettingsAuditlogOnCreateHook(
            Lazy<IAuditlogger<ApplicationSettings>> auditlogger
        )
        {
            _auditlogger = auditlogger;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<ApplicationSettings> context,
            CancellationToken cancellationToken = default)
        {
            return _auditlogger.Value.CreateAuditLogAsync(context.Entity, context.Pristine, cancellationToken);
        }
    }
}
