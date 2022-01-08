using Demo.Domain.Customer;
using Demo.Domain.Invoice;
using Demo.Domain.Invoice.Interfaces;
using Demo.Domain.Invoice.Models;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Services
{
    internal class InvoiceToPdfModelMapper : IInvoiceToPdfModelMapper
    {
        private readonly IDbQuery<Customer> _customerQuery;
        //private readonly IDbQuery<Contact> _contactQuery;
        //private readonly IDbQuery<Item> _itemQuery;

        public InvoiceToPdfModelMapper(
            IDbQuery<Customer> customerQuery//,
                                            //IDbQuery<Contact> contactQuery,
                                            //IDbQuery<Item> itemQuery
        )
        {
            _customerQuery = customerQuery;
            //_contactQuery = contactQuery;
            //_itemQuery = itemQuery;
        }

        public async Task<InvoiceToPdfModel> MapAsync(Invoice invoice, CancellationToken cancellationToken)
        {
            var customer = await _customerQuery.AsQueryable()
                .SingleAsync(x => x.Id == invoice.CustomerId, cancellationToken);
            //var contact = invoice.ContactId.HasValue ?
            //    await _contactQuery.AsQueryable()
            //        .SingleAsync(x => x.Id == invoice.ContactId, cancellationToken) :
            //    null;
            //var itemIds = invoice.InvoiceLines.Where(x => x.ItemId.HasValue).Select(x => x.ItemId);
            //var items = itemIds.Any() ?
            //    await _itemQuery.AsQueryable()
            //        .Where(x => itemIds.Contains(x.Id))
            //        .ToListAsync(cancellationToken) :
            //    new List<Item>();

            var invoiceToPdfModel = new InvoiceToPdfModel
            {
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                CustomerName = customer.Name,
                //ContactName = contact?.LastName,
                InvoiceLines = new List<InvoiceToPdfInvoiceLineModel>()
            };

            foreach (var entityInvoiceLine in invoice.InvoiceLines.OrderBy(x => x.LineNumber))
            {
                //var item = entityInvoiceLine.ItemId.HasValue ?
                //    items.Single(x => x.Id == entityInvoiceLine.ItemId) :
                //    null;
                var modelInvoiceLine = new InvoiceToPdfInvoiceLineModel
                {
                    Quantity = entityInvoiceLine.Quantity,
                    Description = entityInvoiceLine.Description,
                    //ItemDescription = item?.Description
                };
                invoiceToPdfModel.InvoiceLines.Add(modelInvoiceLine);
            }

            return invoiceToPdfModel;
        }
    }
}
