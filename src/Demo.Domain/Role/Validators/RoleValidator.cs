using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;

namespace Demo.Domain.Role.Validators
{
    internal class RoleValidator : AbstractValidator<Role>, Shared.Interfaces.IValidator<Role>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Role> context,
            CancellationToken cancellationToken = default)
        {
            RuleFor(user => user.Name).NotEmpty();

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
