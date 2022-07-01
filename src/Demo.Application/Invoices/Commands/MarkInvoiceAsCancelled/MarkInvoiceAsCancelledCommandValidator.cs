using FluentValidation;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled
{
    public class MarkInvoiceAsCancelledCommandValidator : AbstractValidator<MarkInvoiceAsCancelledCommand>
    {
        public MarkInvoiceAsCancelledCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}