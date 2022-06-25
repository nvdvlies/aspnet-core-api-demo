using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Invoice;
using Demo.Infrastructure.Auditlogging;
using Demo.Infrastructure.Tests.Auditlogging.Helpers;
using Moq;
using Xunit;

namespace Demo.Infrastructure.Tests.Auditlogging
{
    public class InvoiceAuditloggerTests : AuditloggerTestsBase
    {
        private readonly InvoiceAuditlogger _subject;

        public InvoiceAuditloggerTests()
        {
            _subject = new InvoiceAuditlogger(CurrentUserMock.Object, DateTimeMock.Object,
                AuditlogDomainEntityMock.Object);
        }

        [Fact]
        public async Task InvoiceAuditlogger_When_no_changes_are_made_It_should_not_create_auditlog_item()
        {
            // Arrange
            var today = DateTime.Today;
            var previous = new Invoice { InvoiceLines = new List<InvoiceLine> { new() { LineNumber = 1 } } };
            var current = new Invoice { InvoiceLines = new List<InvoiceLine> { new() { LineNumber = 1 } } };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            VerifyCreateAsyncCall(Times.Never);
        }

        [Fact]
        public async Task InvoiceAuditlogger_When_OrderReference_is_modified_It_should_create_auditlog_item()
        {
            // Arrange
            var previous = new Invoice { OrderReference = "X" };
            var current = new Invoice { OrderReference = "Y" };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            Assert.Equal(nameof(Invoice), CapturedAuditlog.EntityName);
            Assert.Single(CapturedAuditlog.AuditlogItems);
            CapturedAuditlog.AssertHasAuditlogItem(nameof(Invoice.OrderReference), previous.OrderReference, current.OrderReference);
            VerifyCreateAsyncCall(Times.Once);
        }

        [Fact]
        public async Task InvoiceAuditlogger_When_invoice_line_is_modified_It_should_create_auditlog_item()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var previous = new Invoice
            {
                InvoiceLines = new List<InvoiceLine> { new() { Id = invoiceLineId, LineNumber = 1 } }
            };
            var current = new Invoice
            {
                InvoiceLines = new List<InvoiceLine> { new() { Id = invoiceLineId, LineNumber = 2 } }
            };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            Assert.Equal(nameof(Invoice), CapturedAuditlog.EntityName);
            Assert.Single(CapturedAuditlog.AuditlogItems);
            CapturedAuditlog.AssertHasAuditlogItem($"{nameof(Invoice.InvoiceLines)}.1.{nameof(InvoiceLine.LineNumber)}", "1", "2");
            VerifyCreateAsyncCall(Times.Once);
        }

        [Fact]
        public async Task InvoiceAuditlogger_When_invoice_line_is_removed_It_should_create_auditlog_item()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var previous = new Invoice
            {
                InvoiceLines = new List<InvoiceLine> { new() { Id = invoiceLineId, LineNumber = 1 } }
            };
            var current = new Invoice();

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            Assert.Equal(nameof(Invoice), CapturedAuditlog.EntityName);
            Assert.Single(CapturedAuditlog.AuditlogItems);
            CapturedAuditlog.AssertHasAuditlogItem($"{nameof(Invoice.InvoiceLines)}.1.{nameof(InvoiceLine.LineNumber)}", "1", "0");
            VerifyCreateAsyncCall(Times.Once);
        }

        [Fact]
        public async Task InvoiceAuditlogger_When_invoice_line_is_added_It_should_create_auditlog_item()
        {
            // Arrange
            var previous = new Invoice();
            var current = new Invoice
            {
                InvoiceLines = new List<InvoiceLine> { new() { LineNumber = 1 } }
            };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            Assert.Equal(nameof(Invoice), CapturedAuditlog.EntityName);
            Assert.Single(CapturedAuditlog.AuditlogItems);
            CapturedAuditlog.AssertHasAuditlogItem($"{nameof(Invoice.InvoiceLines)}.1.{nameof(InvoiceLine.LineNumber)}", "0", "1");
            VerifyCreateAsyncCall(Times.Once);
        }
    }
}
