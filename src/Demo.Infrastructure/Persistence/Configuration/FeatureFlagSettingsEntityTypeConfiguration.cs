using Demo.Domain.FeatureFlagSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class FeatureFlagSettingsEntityTypeConfiguration : IEntityTypeConfiguration<FeatureFlagSettings>
{
    public void Configure(EntityTypeBuilder<FeatureFlagSettings> builder)
    {
        builder.ToTable(nameof(FeatureFlagSettings))
            .HasKey(x => x.Id);

        builder.Property(x => x.Settings)
            .HasColumnType("jsonb");

        builder.Property(p => p.CreatedOn).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.LastModifiedBy);
        builder.Property(p => p.LastModifiedOn);

        builder.UseXminAsConcurrencyToken();
    }
}
