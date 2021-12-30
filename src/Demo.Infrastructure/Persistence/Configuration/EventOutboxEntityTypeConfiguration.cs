using Demo.Events;
using Demo.Infrastructure.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class EventOutboxEntityTypeConfiguration : IEntityTypeConfiguration<EventOutbox>
    {
        public void Configure(EntityTypeBuilder<EventOutbox> builder)
        {
            builder.ToTable(nameof(EventOutbox))
                .HasKey(x => x.Id);

            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => x.LockedUntil);

            builder.Property(x => x.Type)
                .IsRequired();
            builder.Property(x => x.Event)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    x => JsonSerializer.Deserialize<Event>(x, new JsonSerializerOptions
                    {

                    })
                );
            builder.Property(x => x.LockedUntil);
            builder.Property(x => x.LockToken)
                .IsConcurrencyToken();
            builder.Property(x => x.IsPublished)
                .HasDefaultValue(false);
            builder.Property(x => x.Timestamp)
                .IsRowVersion();
        }
    }
}
