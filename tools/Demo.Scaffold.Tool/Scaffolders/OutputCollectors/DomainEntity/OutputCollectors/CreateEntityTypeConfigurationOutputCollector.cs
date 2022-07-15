using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class CreateEntityTypeConfigurationOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var enableSoftDelete = context.Variables.Get<bool>(Constants.EnableSoftDelete);

            changes.Add(new CreateNewClass(
                context.GetEntityTypeConfigurationDirectory(),
                $"{entityName}EntityTypeConfiguration.cs",
                GetTemplate(entityName, enableSoftDelete)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName, bool enableSoftDelete)
        {
            var code = @"
using Demo.Domain.%ENTITY%;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configuration
{
    public class %ENTITY%EntityTypeConfiguration : IEntityTypeConfiguration<%ENTITY%>
    {
        public void Configure(EntityTypeBuilder<%ENTITY%> builder)
        {
            builder.ToTable(nameof(%ENTITY%))
                .HasKey(x => x.Id);

            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);
            %SOFT_DELETE_PROPERTIES%

            builder.UseXminAsConcurrencyToken();
        }
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%SOFT_DELETE_PROPERTIES%", enableSoftDelete
                ? @"builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.DeletedBy);
            builder.Property(p => p.DeletedOn);"
                : null);
            return code;
        }
    }
}
