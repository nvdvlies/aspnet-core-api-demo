using System;
using Demo.Domain.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public static readonly Guid AdministratorRoleId = Guid.Parse("7C20005D-D5F8-4079-AF26-434D69B43C82");
        public static readonly Guid UserRoleId = Guid.Parse("D8A81CD5-D828-47AC-9F72-2E660F43A176");

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

            builder.HasData(new Role { Id = AdministratorRoleId, Name = "Admin", ExternalId = "rol_N4KEnzIMUDaetcyr" },
                new Role { Id = UserRoleId, Name = "User", ExternalId = "rol_OUaEQHOTuugOJHwe" });
        }
    }
}