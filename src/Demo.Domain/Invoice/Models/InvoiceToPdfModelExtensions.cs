using System;
using System.Text;
using System.Text.Json;

namespace Demo.Domain.Invoice.Models;

public static class InvoiceToPdfModelExtensions
{
    public static string GetChecksum(this InvoiceToPdfModel invoiceToPdfModel)
    {
        var json = JsonSerializer.Serialize(invoiceToPdfModel);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }
}
