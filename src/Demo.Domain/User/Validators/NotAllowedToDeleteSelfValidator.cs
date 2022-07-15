using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.User.Validators
{
    internal class NotAllowedToDeleteSelfValidator : IValidator<User>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public NotAllowedToDeleteSelfValidator(ICurrentUserIdProvider currentUserIdProvider)
        {
            _currentUserIdProvider = currentUserIdProvider;
        }

        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<User> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return ValidationResultTask.Ok();
            }

            var isCurrentUser = context.Entity.Id == _currentUserIdProvider.Id;

            if (isCurrentUser)
            {
                return ValidationResultTask.Invalid("Cannot delete own user.");
            }

            return ValidationResultTask.Ok();
        }
    }
}
