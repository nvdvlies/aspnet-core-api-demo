using System.Collections.Generic;
using System.Linq;
using Demo.Application.Roles.Commands.UpdateRole.Dtos;
using FluentValidation;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ExternalId).NotEmpty();
            RuleForEach(role => role.RolePermissions).ChildRules(rolePermission => { rolePermission.RuleFor(x => x.PermissionId).NotEmpty(); });
            RuleFor(role => role.RolePermissions)
                .Must(HasUniquePermissions)
                .WithMessage(_ => "RolePermissions cannot contain duplicate permissions");
        }

        private static bool HasUniquePermissions(IList<UpdateRoleCommandRolePermission> rolePermissions)
        {
            return rolePermissions.GroupBy(x => x.PermissionId).Count() == rolePermissions.Count;
        }
    }
}