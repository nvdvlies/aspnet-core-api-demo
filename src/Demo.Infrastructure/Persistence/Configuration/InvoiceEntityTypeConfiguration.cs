using Demo.Domain.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configurations
{
    public class InvoiceEntityTypeConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable(nameof(Invoice))
                .HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNumber)
                .HasMaxLength(10)
                .HasDefaultValueSql($"CONCAT(YEAR(GETUTCDATE()), NEXT VALUE FOR {Sequences.InvoiceNumber})")
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.InvoiceDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.PaymentTerm);

            builder.Property(t => t.OrderReference)
                .HasMaxLength(50);

            builder.Property(t => t.PdfIsSynced);

            builder.Property(t => t.PdfHashcode);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.InvoiceLines)
                .WithOne(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId);

            builder.Property(x => x.Timestamp).IsRowVersion();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.LastModifiedBy);
            builder.Property(x => x.LastModifiedOn);
            builder.Property(x => x.Deleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedBy);
            builder.Property(x => x.DeletedOn);
        }
    }
}
