using Demo.Domain.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class InvoiceLineEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceLine> builder)
        {
            builder.ToTable(nameof(InvoiceLine))
                .HasKey(x => x.Id);

            builder.Property(x => x.LineNumber)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.SellingPrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(x => x.Invoice)
                .WithMany(x => x.InvoiceLines)
                .HasForeignKey(x => x.InvoiceId)
                .IsRequired();

            //builder.HasOne(x => x.Item)
            //    .WithMany(x => x.InvoiceLines)
            //    .HasForeignKey(x => x.ItemId)
            //    .IsRequired();

            builder.UseXminAsConcurrencyToken();
        }
    }
}