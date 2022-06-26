using System.Text.Json;
using Demo.Domain.FeatureFlagSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class FeatureFlagSettingsEntityTypeConfiguration : IEntityTypeConfiguration<FeatureFlagSettings>
    {
        public void Configure(EntityTypeBuilder<FeatureFlagSettings> builder)
        {
            builder.ToTable(nameof(FeatureFlagSettings))
                .HasKey(x => x.Id);

            builder.Property(x => x.Settings)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions { WriteIndented = true }),
                    x => JsonSerializer.Deserialize<FeatureFlagSettingsSettings>(x, new JsonSerializerOptions())
                );

            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);

            builder.UseXminAsConcurrencyToken();
        }
    }
}
