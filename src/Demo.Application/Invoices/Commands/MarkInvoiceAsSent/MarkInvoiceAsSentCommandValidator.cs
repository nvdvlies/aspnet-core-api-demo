using FluentValidation;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsSent
{
    public class MarkInvoiceAsSentCommandValidator : AbstractValidator<MarkInvoiceAsSentCommand>
    {
        public MarkInvoiceAsSentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}