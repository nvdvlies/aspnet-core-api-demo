using FluentValidation;

namespace Demo.Application.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        public UpdateInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
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
