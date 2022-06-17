using Demo.Domain.OutboxMessage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable(nameof(OutboxMessage))
                .HasKey(x => x.Id);

            builder.HasIndex(x => x.IsSent);
            builder.HasIndex(x => x.LockedUntil);

            builder.Property(x => x.Type)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.Message);
            builder.Property(x => x.LockedUntil);
            builder.Property(x => x.LockToken)
                .HasMaxLength(50)
                .IsConcurrencyToken();
            builder.Property(x => x.IsSent)
                .HasDefaultValue(false);
            builder.Property(x => x.Timestamp)
                .IsRowVersion();
        }
    }
}