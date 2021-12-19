using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.DomainEntity.Validators
{
    internal class NotAllowedToDeleteInvoiceInStatusValidator : BaseValidator, IValidator<Invoice>
    {
        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Invoice> context, CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Delete)
            {
                return CompletedTask;
            }

            var statussesAllowedToDelete = new[] { InvoiceStatus.Draft, InvoiceStatus.Cancelled };
            if (!statussesAllowedToDelete.Contains(context.Pristine.Status))
            {
                return Task.FromResult("Not allowed to delete invoice in current status.".ToValidationMessage());
            }

            return CompletedTask;
        }
    }
}
