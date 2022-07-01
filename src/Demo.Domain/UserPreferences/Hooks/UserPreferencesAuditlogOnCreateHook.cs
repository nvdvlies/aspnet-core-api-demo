using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Hooks
{
    internal class UserPreferencesAuditlogOnCreateHook : IAfterCreate<UserPreferences>
    {
        private readonly Lazy<IAuditlogger<UserPreferences>> _auditlogger;

        public UserPreferencesAuditlogOnCreateHook(
            Lazy<IAuditlogger<UserPreferences>> auditlogger
        )
        {
            _auditlogger = auditlogger;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context,
            CancellationToken cancellationToken)
        {
            await _auditlogger.Value.CreateAuditLogAsync(context.Entity, context.Pristine, cancellationToken);
        }
    }
}