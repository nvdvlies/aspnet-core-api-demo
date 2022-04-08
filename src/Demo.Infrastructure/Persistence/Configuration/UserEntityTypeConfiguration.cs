using Demo.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public static readonly Guid AdministratorUserId = Guid.Parse("08463267-7065-4631-9944-08DA09D992D6");
        public static readonly string AdministratorExternalId = "auth0|08463267-7065-4631-9944-08da09d992d6";

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User))
                .HasKey(x => x.Id);

            builder.Property(x => x.ExternalId)
                .HasMaxLength(50);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Fullname);

            builder.Property(x => x.Fullname)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.GivenName)
                .HasMaxLength(100);

            builder.Property(x => x.FamilyName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.MiddleName)
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(t => t.Gender);

            builder.Property(x => x.BirthDate)
                .HasColumnType("date")
                .IsUtc();

            builder.Property(x => x.ZoneInfo)
                .HasMaxLength(50);

            builder.Property(x => x.Locale)
                .HasMaxLength(10);

            builder.Property(p => p.Timestamp).IsRowVersion();
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.DeletedBy);
            builder.Property(p => p.DeletedOn);

            builder.HasData(new User
            {
                Id = AdministratorUserId,
                ExternalId = AdministratorExternalId,
                FamilyName = "Administrator",
                Fullname = "Administrator",
                Email = "admin@xxxx.xxxx"
            });
        }
    }
}