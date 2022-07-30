using Demo.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer))
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Deleted);

        builder.Property(x => x.Code)
            .HasMaxLength(10)
            .HasDefaultValueSql($"nextval('{Constants.SchemaName}.\"{Sequences.CustomerCode}\"')")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.InvoiceEmailAddress)
            .HasMaxLength(320);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Customer>(x => x.AddressId);

        builder.Property(x => x.CreatedOn).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(64).IsRequired();
        builder.Property(x => x.LastModifiedBy).HasMaxLength(64);
        builder.Property(x => x.LastModifiedOn);
        builder.Property(x => x.Deleted).HasDefaultValue(false);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.DeletedOn);

        builder.UseXminAsConcurrencyToken();
    }
}
