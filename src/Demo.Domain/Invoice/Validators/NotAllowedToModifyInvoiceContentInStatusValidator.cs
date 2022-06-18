using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Invoice.Validators
{
    internal class NotAllowedToModifyInvoiceContentInStatusValidator : IValidator<Invoice>
    {
        private readonly ILogger<NotAllowedToModifyInvoiceContentInStatusValidator> _logger;

        public NotAllowedToModifyInvoiceContentInStatusValidator(
            ILogger<NotAllowedToModifyInvoiceContentInStatusValidator> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Invoice> context,
            CancellationToken cancellationToken = default)
        {
            if (context.EditMode != EditMode.Update)
            {
                return ValidationResultTask.Ok();
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
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Not allowed to change invoice content in current status.");
                    return ValidationResultTask.Invalid("Not allowed to change invoice content in current status.");
                }
            }

            return ValidationResultTask.Ok();
        }
    }
}
