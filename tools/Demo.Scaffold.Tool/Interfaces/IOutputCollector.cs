using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Scaffolders;

namespace Demo.Scaffold.Tool.Interfaces
{
    internal interface IOutputCollector
    {
        IEnumerable<IChange> CollectChanges(ScaffolderContext context);
    }
}