using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.BusinessComponent.Interfaces;
using Demo.Domain.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Auditlogging.Shared
{
    internal abstract class AuditlogBase<T> where T : IEntity
    {
        private readonly ICurrentUser _currentUser;
        private readonly IDateTime _dateTime;
        private readonly IAuditlogBusinessComponent _auditlogBusinessComponent;

        public AuditlogBase(
            ICurrentUser currentUser, 
            IDateTime dateTime,
            IAuditlogBusinessComponent auditlogBusinessComponent
        )
        {
            _currentUser = currentUser;
            _dateTime = dateTime;
            _auditlogBusinessComponent = auditlogBusinessComponent;
        }

        protected abstract List<AuditlogItem> AuditlogItems(T current, T previous);

        public async Task CreateAuditLogAsync(T current, T previous, CancellationToken cancellationToken)
        {
            var auditLogItems = AuditlogItems(current, previous);

            if (auditLogItems?.Count == 0)
            {
                return;
            }

            await _auditlogBusinessComponent.NewAsync(cancellationToken);
            _auditlogBusinessComponent.With(x =>
            {
                x.EntityName = current.GetType().Name;
                x.EntityId = current.Id;
                x.ModifiedBy = _currentUser.Id;
                x.ModifiedOn = _dateTime.UtcNow;
                x.AuditlogItems = auditLogItems;
            });
            await _auditlogBusinessComponent.CreateAsync(cancellationToken);
        }
    }
}
