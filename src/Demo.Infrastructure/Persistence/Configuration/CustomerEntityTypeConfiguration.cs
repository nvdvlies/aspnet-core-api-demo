using Demo.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configurations
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer))
                .HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(10)
                .HasDefaultValueSql($"NEXT VALUE FOR {Constants.SchemaName}.{Sequences.CustomerCode}")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.InvoiceEmailAddress)
                .HasMaxLength(320);

            builder.Property(x => x.Timestamp).IsRowVersion();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.LastModifiedBy);
            builder.Property(x => x.LastModifiedOn);
            builder.Property(x => x.Deleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedBy);
            builder.Property(x => x.DeletedOn);
        }
    }
}
