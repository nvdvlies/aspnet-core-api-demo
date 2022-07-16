using AutoMapper;
using Demo.Application.Invoices.Queries.SearchInvoices.Dtos;
using Demo.Domain.Invoice;

namespace Demo.Application.Invoices.Queries.SearchInvoices;

public class SearchInvoicesQueryMappingProfile : Profile
{
    public SearchInvoicesQueryMappingProfile()
    {
        CreateMap<Invoice, SearchInvoiceDto>();
    }
}
