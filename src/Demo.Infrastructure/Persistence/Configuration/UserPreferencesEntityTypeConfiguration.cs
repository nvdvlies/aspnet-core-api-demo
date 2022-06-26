using System.Text.Json;
using Demo.Domain.UserPreferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class UserPreferencesEntityTypeConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.ToTable(nameof(UserPreferences))
                .HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<UserPreferences>(x => x.Id);

            builder.Property(x => x.Preferences)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, new JsonSerializerOptions { WriteIndented = true }),
                    x => JsonSerializer.Deserialize<UserPreferencesPreferences>(x, new JsonSerializerOptions())
                );

            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);

            builder.UseXminAsConcurrencyToken();
        }
    }
}
