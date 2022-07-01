using FluentValidation;

namespace Demo.Application.Invoices.Commands.CreditInvoice
{
    public class CreditInvoiceCommandValidator : AbstractValidator<CreditInvoiceCommand>
    {
        public CreditInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}