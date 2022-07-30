using Demo.Domain.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable(nameof(Location))
            .HasKey(x => x.Id);

        builder.Property(p => p.DisplayName).HasMaxLength(255).IsRequired();

        builder.Property(p => p.StreetName).HasMaxLength(100).IsRequired();
        builder.Property(p => p.HouseNumber).HasMaxLength(10).IsRequired();
        builder.Property(p => p.PostalCode).HasMaxLength(10).IsRequired();
        builder.Property(p => p.City).HasMaxLength(100).IsRequired();
        builder.Property(p => p.CountryCode).HasMaxLength(3).IsRequired();

        builder.Property(p => p.Latitude);
        builder.Property(p => p.Longitude);

        builder.Property(p => p.CreatedOn).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.LastModifiedBy);
        builder.Property(p => p.LastModifiedOn);

        builder.UseXminAsConcurrencyToken();
    }
}
