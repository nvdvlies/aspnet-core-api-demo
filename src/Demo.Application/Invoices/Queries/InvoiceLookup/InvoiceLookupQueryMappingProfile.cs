using AutoMapper;
using Demo.Application.Invoices.Queries.InvoiceLookup.Dtos;
using Demo.Domain.Invoice;

namespace Demo.Application.Invoices.Queries.InvoiceLookup
{
    public class InvoiceLookupQueryMappingProfile : Profile
    {
        public InvoiceLookupQueryMappingProfile()
        {
            CreateMap<Invoice, InvoiceLookupDto>();
        }
    }
}
