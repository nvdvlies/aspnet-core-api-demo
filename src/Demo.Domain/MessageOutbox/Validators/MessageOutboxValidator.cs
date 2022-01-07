using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.MessageOutbox.Validators
{
    internal class MessageOutboxValidator : AbstractValidator<MessageOutbox>, Shared.Interfaces.IValidator<MessageOutbox>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<MessageOutbox> context, CancellationToken cancellationToken = default)
        {
            RuleFor(customer => customer.Type).NotEmpty();
            RuleFor(customer => customer.Message).NotEmpty();

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
