using System;
using System.IO;

namespace Demo.Scaffold.Tool.Changes;

internal class AddUsingStatementToExistingClass : BaseChange, IChange
{
    public AddUsingStatementToExistingClass(string directory, string fileName, string content)
        : base(directory, fileName, content)
    {
    }

    public ModificationTypes ModificationType => ModificationTypes.Update;

    public void Apply()
    {
        var content = File.ReadAllText(DirectoryAndFileName);
        content = $"{Content}{Environment.NewLine}{content}";
        File.WriteAllText(DirectoryAndFileName, content);
    }
}
