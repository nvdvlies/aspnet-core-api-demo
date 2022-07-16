using Demo.Domain.Role;
using Demo.Domain.Role.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class PermissionTypeConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission))
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(x => x.PermissionGroup)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.PermissionGroupId)
            .IsRequired();

        builder.UseXminAsConcurrencyToken();

        builder.HasData(PermissionsSeed.All);
    }
}
