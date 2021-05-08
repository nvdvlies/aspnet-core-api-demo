using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Validators
{
    internal class NotAllowedToDeleteInvoiceInStatusValidator : IValidator<Invoice>
    {
        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return Task.FromResult<IEnumerable<ValidationMessage>>(null);
            }

            var statussesAllowedToDelete = new[] { InvoiceStatus.Draft, InvoiceStatus.Cancelled };
            if (!statussesAllowedToDelete.Contains(context.Pristine.Status))
            {
                return Task.FromResult("Not allowed to delete invoice in current status.".ToValidationMessage());
            }

            return Task.FromResult<IEnumerable<ValidationMessage>>(null);
        }
    }
}
