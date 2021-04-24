using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Scaffolders;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Interfaces
{
    internal interface IOutputCollector
    {
        IEnumerable<IChange> CollectChanges(ScaffolderContext context);
    }
}
