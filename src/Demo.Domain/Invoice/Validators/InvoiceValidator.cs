using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;

namespace Demo.Domain.Invoice.Validators
{
    internal class InvoiceValidator : AbstractValidator<Invoice>, Shared.Interfaces.IValidator<Invoice>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken = default)
        {
            RuleFor(invoice => invoice.CustomerId).NotEmpty();
            RuleFor(invoice => invoice.InvoiceDate).GreaterThan(DateTime.MinValue);
            RuleFor(invoice => invoice.OrderReference).NotEmpty();
            RuleFor(invoice => invoice.InvoiceLines).NotEmpty();
            RuleForEach(invoice => invoice.InvoiceLines).ChildRules(invoiceLine =>
            {
                invoiceLine.RuleFor(x => x.LineNumber).NotEmpty();
                invoiceLine.RuleFor(x => x.Quantity).NotEmpty();
                invoiceLine.RuleFor(x => x.Description).NotEmpty();
            });

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
