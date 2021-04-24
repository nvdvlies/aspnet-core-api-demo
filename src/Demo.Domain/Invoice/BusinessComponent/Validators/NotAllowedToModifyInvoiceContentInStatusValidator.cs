using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Validators
{
    internal class NotAllowedToModifyInvoiceContentInStatusValidator : IValidator<Invoice>
    {
        private readonly ILogger<NotAllowedToModifyInvoiceContentInStatusValidator> _logger;

        public NotAllowedToModifyInvoiceContentInStatusValidator(ILogger<NotAllowedToModifyInvoiceContentInStatusValidator> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Update)
            {
                return Task.FromResult<IEnumerable<ValidationMessage>>(null);
            }

            var statussesAllowedToModifyInvoiceContent = new[] { InvoiceStatus.Draft };
            if (!statussesAllowedToModifyInvoiceContent.Contains(context.Pristine.Status))
            {
                try
                {
                    context.Entity.InvoiceDate.Should().Be(context.Pristine.InvoiceDate);
                    context.Entity.CustomerId.Should().Be(context.Pristine.CustomerId);
                    context.Entity.InvoiceLines.Should().BeEquivalentTo(context.Pristine.InvoiceLines,
                        options => options
                            .Excluding(x => x.Invoice)
                            //.Excluding(x => x.Item)
                            .Excluding(x => x.Timestamp)
                    );
                }
                catch (System.Exception ex)
                {
                    _logger.LogWarning(ex, "De factuur inhoud wijzigen is niet toegestaan in de huidige status.");
                    return Task.FromResult("De factuur inhoud wijzigen is niet toegestaan in de huidige status.".AsEnumerableOfValidationMessages());
                }
            }

            return Task.FromResult<IEnumerable<ValidationMessage>>(null);
        }
    }
}
