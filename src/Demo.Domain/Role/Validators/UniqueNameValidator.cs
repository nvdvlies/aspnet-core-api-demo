using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Demo.Domain.Role.Validators
{
    internal class UniqueNameValidator : AbstractValidator<Role>, Shared.Interfaces.IValidator<Role>
    {
        private readonly IDbQuery<Role> _roleQuery;

        public UniqueNameValidator(IDbQuery<Role> roleQuery)
        {
            _roleQuery = roleQuery;
        }

        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Role> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode == EditMode.Delete)
            {
                return ValidationResult.Ok();
            }

            if (context.IsPropertyDirty(x => x.Name))
            {
                var alreadyExists = await _roleQuery.AsQueryable()
                    .Where(x => x.Id != context.Entity.Id)
                    .Where(x => x.Name == context.Entity.Name)
                    .AnyAsync(cancellationToken);

                if (alreadyExists)
                {
                    return ValidationResult.Invalid($"A role with name '{context.Entity.Name}' already exists.");
                }
            }

            return ValidationResult.Ok();
        }
    }
}
