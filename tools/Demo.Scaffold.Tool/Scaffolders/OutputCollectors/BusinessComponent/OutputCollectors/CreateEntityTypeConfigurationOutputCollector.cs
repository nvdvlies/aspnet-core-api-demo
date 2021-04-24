using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors
{
    internal class CreateEntityTypeConfigurationOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: context.GetEntityTypeConfigurationDirectory(),
                fileName: $"{entityName}EntityTypeConfiguration.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
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

            builder.Property(p => p.Timestamp).IsRowVersion();
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.LastModifiedBy);
            builder.Property(p => p.LastModifiedOn);
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.DeletedBy);
            builder.Property(p => p.DeletedOn);
        }
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
