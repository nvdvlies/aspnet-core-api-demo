using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role.Seed;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;

namespace Demo.Domain.User.Validators;

internal class DefaultAdministratorRequiresAdminRoleValidator : IValidator<User>
{
    public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<User> context,
        CancellationToken cancellationToken = default)
    {
        if (context.EditMode != EditMode.Update)
        {
            return ValidationResultTask.Ok();
        }

        var isDefaultAdministrator = context.Entity.Id == DefaultAdministratorUser.UserId;

        if (isDefaultAdministrator)
        {
            var hasAdminRole = context.Entity.UserRoles.Any(x => x.RoleId == AdministratorRole.RoleId);
            if (!hasAdminRole)
            {
                return ValidationResultTask.Invalid(
                    "Administrator user requires admin role.");
            }
        }

        return ValidationResultTask.Ok();
    }
}
