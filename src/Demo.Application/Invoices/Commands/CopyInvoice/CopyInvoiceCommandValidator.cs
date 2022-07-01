using FluentValidation;

namespace Demo.Application.Invoices.Commands.CopyInvoice
{
    public class CopyInvoiceCommandValidator : AbstractValidator<CopyInvoiceCommand>
    {
        public CopyInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}