using Demo.Domain.Role;
using Demo.Domain.Role.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class PermissionGroupTypeConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable(nameof(PermissionGroup))
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(x => x.Permissions)
            .WithOne(x => x.PermissionGroup)
            .HasForeignKey(x => x.PermissionGroupId);

        builder.UseXminAsConcurrencyToken();

        builder.HasData(PermissionGroupsSeed.All);
    }
}
