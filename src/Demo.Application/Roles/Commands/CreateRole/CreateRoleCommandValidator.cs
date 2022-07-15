using System.Collections.Generic;
using System.Linq;
using Demo.Application.Roles.Commands.CreateRole.Dtos;
using FluentValidation;

namespace Demo.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(role => role.Name).NotEmpty();
            RuleFor(role => role.ExternalId).NotEmpty();
            RuleForEach(role => role.RolePermissions).ChildRules(rolePermission =>
            {
                rolePermission.RuleFor(x => x.PermissionId).NotEmpty();
            });
            RuleFor(role => role.RolePermissions)
                .Must(HasUniquePermissions)
                .WithMessage(_ => "RolePermissions cannot contain duplicate permissions");
        }

        private static bool HasUniquePermissions(IList<CreateRoleCommandRolePermission> rolePermissions)
        {
            return rolePermissions.GroupBy(x => x.PermissionId).Count() == rolePermissions.Count;
        }
    }
}
