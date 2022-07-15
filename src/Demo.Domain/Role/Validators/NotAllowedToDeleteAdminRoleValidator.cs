using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role.Seed;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Role.Validators
{
    internal class NotAllowedToDeleteAdminRoleValidator : IValidator<Role>
    {
        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Role> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return ValidationResultTask.Ok();
            }

            var isAdministratorRole = context.Entity.Id == AdministratorRole.RoleId;

            if (isAdministratorRole)
            {
                return ValidationResultTask.Invalid("Cannot delete administrator.");
            }

            return ValidationResultTask.Ok();
        }
    }
}