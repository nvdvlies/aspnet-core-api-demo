using Demo.Domain.Auditlog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class AuditlogItemEntityTypeConfiguration : IEntityTypeConfiguration<AuditlogItem>
    {
        public void Configure(EntityTypeBuilder<AuditlogItem> builder)
        {
            builder.ToTable(nameof(AuditlogItem))
                .HasKey(x => x.Id);

            builder.Property(t => t.PropertyName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.Type)
                .IsRequired();

            builder.Property(t => t.CurrentValueAsString);

            builder.Property(t => t.PreviousValueAsString);

            builder.Property(p => p.Timestamp)
                .IsRowVersion();

            builder.HasOne(x => x.Auditlog)
                .WithMany(x => x.AuditlogItems)
                .HasForeignKey(x => x.AuditlogId);

            builder.HasOne(x => x.ParentAuditlogItem)
                .WithMany(x => x.AuditlogItems)
                .HasForeignKey(x => x.ParentAuditlogItemId);
        }
    }
}
