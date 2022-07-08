using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Seed;
using Microsoft.EntityFrameworkCore;

namespace Demo.Domain.User.Validators
{
    internal class NotAllowedToDeleteAdministratorValidator : IValidator<User>
    {
        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<User> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return ValidationResultTask.Ok();
            }

            var isDefaultAdministrator = context.Entity.Id == DefaultAdministratorUser.UserId;

            if (isDefaultAdministrator)
            {
                return ValidationResultTask.Invalid(
                    "Cannot delete default administrator.");
            }

            return ValidationResultTask.Ok();
        }
    }
}