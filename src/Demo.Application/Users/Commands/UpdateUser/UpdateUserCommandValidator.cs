using System.Collections.Generic;
using System.Linq;
using Demo.Application.Users.Commands.UpdateUser.Dtos;
using FluentValidation;

namespace Demo.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.FamilyName).NotEmpty();
            RuleFor(user => user.UserRoles).NotEmpty();
            RuleForEach(user => user.UserRoles).ChildRules(userRole => { userRole.RuleFor(x => x.RoleId).NotEmpty(); });
            RuleFor(user => user.UserRoles)
                .Must(userRoles => HasUniqueRoles(userRoles))
                .WithMessage(x => "UserRoles cannot contain duplicate roles");
        }

        private static bool HasUniqueRoles(IEnumerable<UpdateUserCommandUserRoleDto> userRoles)
        {
            return userRoles.GroupBy(x => x.RoleId).Count() == userRoles.Count();
        }
    }
}
