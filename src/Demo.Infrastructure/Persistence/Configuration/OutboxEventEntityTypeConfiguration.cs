using Demo.Domain.OutboxEvent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class OutboxEventEntityTypeConfiguration : IEntityTypeConfiguration<OutboxEvent>
    {
        public void Configure(EntityTypeBuilder<OutboxEvent> builder)
        {
            builder.ToTable(nameof(OutboxEvent))
                .HasKey(x => x.Id);

            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => x.LockedUntil);

            builder.Property(x => x.Type)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.Event);
            builder.Property(x => x.LockedUntil);
            builder.Property(x => x.LockToken)
                .HasMaxLength(50)
                .IsConcurrencyToken();
            builder.Property(x => x.IsPublished)
                .HasDefaultValue(false);
            builder.Property(x => x.Timestamp)
                .IsRowVersion();
        }
    }
}
