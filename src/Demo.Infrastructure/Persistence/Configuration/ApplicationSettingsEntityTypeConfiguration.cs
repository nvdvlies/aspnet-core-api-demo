using Demo.Domain.ApplicationSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class ApplicationSettingsEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationSettings>
    {
        public void Configure(EntityTypeBuilder<ApplicationSettings> builder)
        {
            builder.ToTable(nameof(ApplicationSettings))
                .HasKey(x => x.Id);

            builder.Property(x => x.Settings)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    x => JsonSerializer.Deserialize<ApplicationSettingsSettings>(x, new JsonSerializerOptions
                    {

                    })
                );

            builder.Property(x => x.Timestamp).IsRowVersion();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.LastModifiedBy);
            builder.Property(x => x.LastModifiedOn);
        }
    }
}
