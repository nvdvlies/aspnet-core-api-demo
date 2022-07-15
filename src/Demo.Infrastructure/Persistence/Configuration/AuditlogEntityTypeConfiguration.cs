using Demo.Domain.Auditlog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class AuditlogEntityTypeConfiguration : IEntityTypeConfiguration<Auditlog>
    {
        public void Configure(EntityTypeBuilder<Auditlog> builder)
        {
            builder.ToTable(nameof(Auditlog))
                .HasKey(x => x.Id);

            builder.HasIndex(x => x.EntityName);
            builder.HasIndex(x => x.EntityId);

            builder.Property(x => x.EntityName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.ModifiedOn)
                .IsRequired();

            builder.Property(x => x.AuditlogItems)
                .HasColumnType("jsonb");

            builder.UseXminAsConcurrencyToken();
        }
    }
}
