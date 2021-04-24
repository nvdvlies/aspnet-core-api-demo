using Demo.Domain.ApplicationSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Demo.Infrastructure.Persistence.Configurations
{
    public class ApplicationSettingsEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationSettings>
    {
        public void Configure(EntityTypeBuilder<ApplicationSettings> builder)
        {
            builder.ToTable(nameof(ApplicationSettings))
                .HasKey(x => x.Id);

            builder.Property(x => x.Settings)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x, new JsonSerializerSettings { 
                        NullValueHandling = NullValueHandling.Ignore,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    }),
                    x => JsonConvert.DeserializeObject<ApplicationSettingsSettings>(x, new JsonSerializerSettings { 
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
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
