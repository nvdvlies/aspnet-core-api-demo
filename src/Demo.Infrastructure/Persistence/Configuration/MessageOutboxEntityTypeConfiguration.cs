using Demo.Domain.MessageOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class MessageOutboxEntityTypeConfiguration : IEntityTypeConfiguration<MessageOutbox>
    {
        public void Configure(EntityTypeBuilder<MessageOutbox> builder)
        {
            builder.ToTable(nameof(MessageOutbox))
                .HasKey(x => x.Id);

            builder.HasIndex(x => x.IsSent);
            builder.HasIndex(x => x.LockedUntil);

            builder.Property(x => x.Type)
                .IsRequired();
            builder.Property(x => x.Message);
            builder.Property(x => x.LockedUntil);
            builder.Property(x => x.LockToken)
                .IsConcurrencyToken();
            builder.Property(x => x.IsSent)
                .HasDefaultValue(false);
            builder.Property(x => x.Timestamp)
                .IsRowVersion();
        }
    }
}
