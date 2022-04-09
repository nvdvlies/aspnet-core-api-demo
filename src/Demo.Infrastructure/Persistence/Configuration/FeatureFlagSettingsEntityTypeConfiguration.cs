using Demo.Domain.FeatureFlagSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text.Json;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class FeatureFlagSettingsEntityTypeConfiguration : IEntityTypeConfiguration<FeatureFlagSettings>
    {
        public void Configure(EntityTypeBuilder<FeatureFlagSettings> builder)
        {
            builder.ToTable(nameof(FeatureFlagSettings))
                .HasKey(x => x.Id);

            builder.Property(x => x.FeatureFlags)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    x => JsonSerializer.Deserialize<List<FeatureFlag>>(x, new JsonSerializerOptions
                    {

                    })
                );

            builder.Property(p => p.Timestamp).IsRowVersion();
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);
        }
    }
}