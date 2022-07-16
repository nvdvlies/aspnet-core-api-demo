using System.Collections.Generic;
using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Customer;

public class Customer : SoftDeleteEntity, IQueryableEntity
{
    [JsonInclude] public int Code { get; internal set; }

    public string Name { get; set; }
    public string InvoiceEmailAddress { get; set; }
    public List<Invoice.Invoice> Invoices { get; set; }
}
