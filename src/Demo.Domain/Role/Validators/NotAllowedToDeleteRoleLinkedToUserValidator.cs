using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Domain.Role.Validators
{
    internal class NotAllowedToDeleteRoleLinkedToUserValidator : IValidator<Role>
    {
        private readonly IDbQuery<User.User> _userQuery;

        public NotAllowedToDeleteRoleLinkedToUserValidator(IDbQuery<User.User> userQuery)
        {
            _userQuery = userQuery;
        }

        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Role> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return ValidationResult.Ok();
            }

            var isRoleInUse = await _userQuery.AsQueryable()
                .Where(user => user.UserRoles.Any(userRole => userRole.RoleId == context.Entity.Id))
                .AnyAsync(cancellationToken);

            if (isRoleInUse)
            {
                return ValidationResult.Invalid(
                    "Cannot delete role, because one or more users are linked to this role.");
            }

            return ValidationResult.Ok();
        }
    }
}
