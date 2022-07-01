using FluentValidation;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsPaid
{
    public class MarkInvoiceAsPaidCommandValidator : AbstractValidator<MarkInvoiceAsPaidCommand>
    {
        public MarkInvoiceAsPaidCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}