using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Invoice.Validators
{
    internal class NotAllowedToDeleteInvoiceInStatusValidator : IValidator<Invoice>
    {
        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return ValidationResultTask.Ok();
            }

            var statussesAllowedToDelete = new[] { InvoiceStatus.Draft, InvoiceStatus.Cancelled };
            if (!statussesAllowedToDelete.Contains(context.Pristine.Status))
            {
                return ValidationResultTask.Invalid("Not allowed to delete invoice in current status.");
            }

            return ValidationResultTask.Ok();
        }
    }
}