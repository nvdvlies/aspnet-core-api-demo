using Demo.Domain.ApplicationSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class ApplicationSettingsEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationSettings>
    {
        public void Configure(EntityTypeBuilder<ApplicationSettings> builder)
        {
            builder.ToTable(nameof(ApplicationSettings))
                .HasKey(x => x.Id);

            builder.Property(x => x.Settings)
                .HasColumnType("jsonb");

            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).HasMaxLength(64).IsRequired();
            builder.Property(x => x.LastModifiedBy).HasMaxLength(64);
            builder.Property(x => x.LastModifiedOn);

            builder.UseXminAsConcurrencyToken();
        }
    }
}
