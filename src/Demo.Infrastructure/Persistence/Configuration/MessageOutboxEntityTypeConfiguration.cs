using Demo.Domain.MessageOutbox;
using Demo.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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
            builder.Property(x => x.Message)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    x => JsonSerializer.Deserialize<Message>(x, new JsonSerializerOptions
                    {

                    })
                );
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
