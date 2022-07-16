using Demo.Domain.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role))
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.CreatedOn).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.LastModifiedBy);
        builder.Property(p => p.LastModifiedOn);
        builder.Property(p => p.Deleted).HasDefaultValue(false);
        builder.Property(p => p.DeletedBy);
        builder.Property(p => p.DeletedOn);

        builder.UseXminAsConcurrencyToken();
    }
}
