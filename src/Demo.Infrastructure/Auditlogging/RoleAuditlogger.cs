using System.Collections.Generic;
using System.Linq;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;

namespace Demo.Infrastructure.Auditlogging
{
    internal class RoleAuditlogger : AuditloggerBase<Role>, IAuditlogger<Role>
    {
        public RoleAuditlogger(
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUserIdProvider, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(Role current, Role previous)
        {
            return new AuditlogBuilder<Role>()
                .WithProperty(x => x.Name)
                .WithProperty(x => x.ExternalId)
                .WithProperty(x => x.RolePermissions.Select(y => y.PermissionId).ToList(), nameof(Role.RolePermissions))
                .Build(current, previous);
        }
    }
}
