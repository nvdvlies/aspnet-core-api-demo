using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.OutboxEvent.Validators
{
    internal class OutboxEventValidator : AbstractValidator<OutboxEvent>, Shared.Interfaces.IValidator<OutboxEvent>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<OutboxEvent> context, CancellationToken cancellationToken = default)
        {
            RuleFor(customer => customer.Type).NotEmpty();
            RuleFor(customer => customer.Event).NotEmpty();

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
