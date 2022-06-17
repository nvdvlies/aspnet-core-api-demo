using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class AddDbSetToDbContextOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var collectionName = context.Variables.Get<string>(Constants.CollectionName);

            changes.Add(new UpdateExistingClassAtMarker(
                context.GetPersistenceDirectory(),
                "ApplicationDbContext.cs",
                Tool.Constants.ScaffoldMarkerDbSet,
                GetTemplate(entityName, collectionName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName, string collectionName)
        {
            var code = @"       public DbSet<%ENTITY%> %COLLECTIONNAME% { get; set; }";

            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%COLLECTIONNAME%", collectionName);
            return code;
        }
    }
}