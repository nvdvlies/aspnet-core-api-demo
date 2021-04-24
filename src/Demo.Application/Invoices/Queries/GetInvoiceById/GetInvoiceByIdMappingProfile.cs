using AutoMapper;
using Demo.Application.Invoices.Queries.GetInvoiceById.Dtos;
using Demo.Domain.Invoice;

namespace Demo.Application.Shared.Mappings
{
    public partial class GetInvoiceByIdMappingProfile : Profile
    {
        public GetInvoiceByIdMappingProfile()
        {
            CreateMap<InvoiceLine, InvoiceLineDto>();
            CreateMap<Invoice, InvoiceDto>();
        }
    }
}
