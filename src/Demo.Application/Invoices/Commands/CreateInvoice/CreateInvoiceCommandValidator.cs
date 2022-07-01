using FluentValidation;

namespace Demo.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.InvoiceDate).NotEmpty();
            RuleFor(x => x.OrderReference).NotEmpty();
            RuleFor(x => x.InvoiceLines).NotEmpty();
            RuleForEach(x => x.InvoiceLines).ChildRules(invoiceLine =>
            {
                invoiceLine.RuleFor(x => x.Quantity).NotEmpty();
                invoiceLine.RuleFor(x => x.Description).NotEmpty();
            });
        }
    }
}