using System.Collections.Generic;
using System.Linq;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User;
using Demo.Infrastructure.Auditlogging.Shared;

namespace Demo.Infrastructure.Auditlogging
{
    internal class UserAuditlogger : AuditloggerBase<User>, IAuditlogger<User>
    {
        public UserAuditlogger(
            ICurrentUser currentUser,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(User current, User previous)
        {
            return new AuditlogBuilder<User>()
                .WithProperty(x => x.Fullname)
                .WithProperty(x => x.BirthDate, AuditlogType.DateOnly)
                .WithProperty(x => x.Gender)
                .WithProperty(x => x.Email)
                .WithProperty(x => x.Locale)
                .WithProperty(x => x.ZoneInfo)
                .WithProperty(c => c.UserRoles.Select(x => x.RoleId).ToList(), nameof(User.UserRoles))
                .Build(current, previous);
        }
    }
}