using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.User.Validators
{
    internal class UniqueEmailValidator : AbstractValidator<User>, Shared.Interfaces.IValidator<User>
    {
        private readonly IDbQuery<User> _userQuery;

        public UniqueEmailValidator(IDbQuery<User> userQuery)
        {
            _userQuery = userQuery;
        }

        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<User> context, CancellationToken cancellationToken = default)
        {
            if (context.EditMode == EditMode.Delete)
            {
                return ValidationResult.Ok();
            }

            if (context.IsPropertyDirty(x => x.Email))
            {
                var alreadyExists = await _userQuery.AsQueryable()
                    .Where(x => x.Id != context.Entity.Id)
                    .Where(x => x.Email == context.Entity.Email)
                    .AnyAsync(cancellationToken);

                if (alreadyExists)
                {
                    return ValidationResult.Invalid($"A user with email '{context.Entity.Email}' already exists.");
                }
            }

            return ValidationResult.Ok();
        }
    }
}
