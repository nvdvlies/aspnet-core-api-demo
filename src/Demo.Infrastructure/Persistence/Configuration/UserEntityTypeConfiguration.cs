using Demo.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User))
            .HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .HasMaxLength(50);

        builder.HasIndex(x => x.Email);
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

        builder.Property(t => t.UserType)
            .HasDefaultValue(UserType.Regular)
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
