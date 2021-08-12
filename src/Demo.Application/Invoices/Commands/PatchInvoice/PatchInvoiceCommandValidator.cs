using FluentValidation;

namespace Demo.Application.Invoices.Commands.PatchInvoice
{
    public class PatchInvoiceCommandValidator : AbstractValidator<PatchInvoiceCommand>
    {
        public PatchInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.PatchDocument.Operations).NotEmpty();
        }
    }
}