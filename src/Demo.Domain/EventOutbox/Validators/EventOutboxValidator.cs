using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.EventOutbox.Validators
{
    internal class EventOutboxValidator : AbstractValidator<EventOutbox>, Shared.Interfaces.IValidator<EventOutbox>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<EventOutbox> context, CancellationToken cancellationToken = default)
        {
            RuleFor(customer => customer.Type).NotEmpty();
            RuleFor(customer => customer.Event).NotEmpty();

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
