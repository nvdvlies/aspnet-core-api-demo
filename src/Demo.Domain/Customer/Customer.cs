using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Demo.Domain.Customer
{
    public partial class Customer : SoftDeleteEntity, IQueryableEntity
    {
        [JsonInclude]
        public int Code { get; internal set; }
        public string Name { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public List<Invoice.Invoice> Invoices { get; set; }
    }
}
