using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;

namespace Demo.Domain.User.Validators
{
    internal class UserValidator : AbstractValidator<User>, Shared.Interfaces.IValidator<User>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<User> context,
            CancellationToken cancellationToken = default)
        {
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.Email).EmailAddress();
            RuleFor(user => user.FamilyName).NotEmpty();
            RuleFor(user => user.UserRoles).NotEmpty();
            RuleForEach(user => user.UserRoles).ChildRules(userRole => { userRole.RuleFor(x => x.RoleId).NotEmpty(); });
            RuleFor(user => user.UserRoles)
                .Must(userRoles => HasUniqueRoles(userRoles))
                .WithMessage(x => "UserRoles cannot contain duplicate roles");

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }

        private static bool HasUniqueRoles(IEnumerable<UserRole> userRoles)
        {
            return userRoles.GroupBy(x => x.RoleId).Count() == userRoles.Count();
        }
    }
}
