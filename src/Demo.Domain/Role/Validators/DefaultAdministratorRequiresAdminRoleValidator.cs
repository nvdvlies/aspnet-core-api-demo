using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role.Seed;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Role.Validators;

internal class AdminRoleRequiresRoleReadWritePermissionValidator : IValidator<Role>
{
    public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Role> context,
        CancellationToken cancellationToken = default)
    {
        if (context.EditMode != EditMode.Update)
        {
            return ValidationResultTask.Ok();
        }

        var isAdministratorRole = context.Entity.Id == AdministratorRole.RoleId;

        if (isAdministratorRole)
        {
            var requiredRoleIds = new[] { PermissionsSeed.RolesRead.Id, PermissionsSeed.RolesWrite.Id };
            var hasAllRequiredRoles = requiredRoleIds.All(requiredRoleId =>
                context.Entity.RolePermissions.Any(rolePermission =>
                    rolePermission.PermissionId == requiredRoleId));

            if (!hasAllRequiredRoles)
            {
                return ValidationResultTask.Invalid(
                    $"Administrator user requires permissions: {PermissionsSeed.RolesRead.Name}, {PermissionsSeed.RolesWrite.Name}.");
            }
        }

        return ValidationResultTask.Ok();
    }
}
