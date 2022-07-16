using FluentValidation;

namespace Demo.Application.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
