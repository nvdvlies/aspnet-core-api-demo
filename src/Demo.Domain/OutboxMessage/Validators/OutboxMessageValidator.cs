using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;

namespace Demo.Domain.OutboxMessage.Validators
{
    internal class OutboxMessageValidator : AbstractValidator<OutboxMessage>,
        Shared.Interfaces.IValidator<OutboxMessage>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<OutboxMessage> context,
            CancellationToken cancellationToken = default)
        {
            RuleFor(customer => customer.Type).NotEmpty();
            RuleFor(customer => customer.Message).NotEmpty();

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}