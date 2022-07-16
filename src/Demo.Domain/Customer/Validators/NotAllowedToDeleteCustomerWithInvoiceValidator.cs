using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Domain.Customer.Validators;

internal class NotAllowedToDeleteCustomerWithInvoiceValidator : IValidator<Customer>
{
    private readonly IDbQuery<Invoice.Invoice> _invoiceQuery;

    public NotAllowedToDeleteCustomerWithInvoiceValidator(IDbQuery<Invoice.Invoice> invoiceQuery)
    {
        _invoiceQuery = invoiceQuery;
    }

    public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Customer> context,
        CancellationToken cancellationToken = default)
    {
        if (context.EditMode != EditMode.Delete)
        {
            return ValidationResult.Ok();
        }

        var invoiceStatussesWhichAllowDeletionOfCustomer = new[] { InvoiceStatus.Cancelled };

        var hasInvoices = await _invoiceQuery.AsQueryable()
            .Where(x => x.CustomerId == context.Entity.Id)
            .Where(x => !invoiceStatussesWhichAllowDeletionOfCustomer.Contains(x.Status))
            .AnyAsync(cancellationToken);

        if (hasInvoices)
        {
            return ValidationResult.Invalid(
                "Cannot delete customer, because one or more invoices are linked to this customer.");
        }

        return ValidationResult.Ok();
    }
}
