using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Auditlogging.Shared
{
    internal abstract class AuditloggerBase<T> where T : IEntity
    {
        private readonly IAuditlogDomainEntity _auditlogDomainEntity;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IDateTime _dateTime;

        public AuditloggerBase(
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _dateTime = dateTime;
            _auditlogDomainEntity = auditlogDomainEntity;
        }

        protected abstract List<AuditlogItem> AuditlogItems(T current, T previous);

        public async Task CreateAuditLogAsync(T current, T previous, CancellationToken cancellationToken)
        {
            var auditLogItems = AuditlogItems(current, previous);

            if (auditLogItems?.Count == 0)
            {
                return;
            }

            await _auditlogDomainEntity.NewAsync(cancellationToken);
            _auditlogDomainEntity.With(x =>
            {
                x.EntityName = current.GetType().Name;
                x.EntityId = current.Id;
                x.ModifiedBy = _currentUserIdProvider.Id;
                x.ModifiedOn = _dateTime.UtcNow;
                x.AuditlogItems = auditLogItems;
            });
            await _auditlogDomainEntity.CreateAsync(cancellationToken);
        }
    }
}
