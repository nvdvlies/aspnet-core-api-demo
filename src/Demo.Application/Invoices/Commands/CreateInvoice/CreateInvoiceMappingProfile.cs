using AutoMapper;
using Demo.Application.Invoices.Commands.CreateInvoice.Dtos;
using Demo.Domain.Invoice;

namespace Demo.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceMappingProfile : Profile
    {
        public CreateInvoiceMappingProfile()
        {
            CreateMap<CreateInvoiceCommandInvoiceLine, InvoiceLine>()
                .ForMember(x => x.LineNumber, opt => opt.Ignore())
                .ForMember(x => x.InvoiceId, opt => opt.Ignore())
                .ForMember(x => x.Invoice, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.xmin, opt => opt.Ignore());

            CreateMap<CreateInvoiceCommand, Invoice>()
                .ForMember(x => x.InvoiceNumber, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt => opt.Ignore())
                .ForMember(x => x.PdfIsSynced, opt => opt.Ignore())
                .ForMember(x => x.PdfChecksum, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.xmin, opt => opt.Ignore());
        }
    }
}