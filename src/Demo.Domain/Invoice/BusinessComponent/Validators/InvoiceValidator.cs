using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Validators
{
    internal class InvoiceValidator : AbstractValidator<Invoice>, Shared.Interfaces.IValidator<Invoice>
    {
        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken = default)
        {
            RuleFor(invoice => invoice.CustomerId).NotEmpty();
            RuleFor(invoice => invoice.InvoiceDate).GreaterThan(DateTime.MinValue);
            RuleFor(invoice => invoice.OrderReference).NotEmpty();
            RuleFor(invoice => invoice.InvoiceLines).NotEmpty();
            RuleForEach(invoice => invoice.InvoiceLines).ChildRules(invoiceLine => {
                invoiceLine.RuleFor(x => x.Quantity).GreaterThan(0);
            });

            var validationResult = await ValidateAsync(context.Entity, cancellationToken);
            return validationResult.ToValidationMessage();
        }
    }
}
