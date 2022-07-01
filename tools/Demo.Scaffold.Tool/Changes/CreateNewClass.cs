using System.IO;

namespace Demo.Scaffold.Tool.Changes
{
    internal class CreateNewClass : BaseChange, IChange
    {
        public CreateNewClass(string directory, string fileName, string content)
            : base(directory, fileName, content)
        {
        }

        public ModificationTypes ModificationType => ModificationTypes.Create;

        public void Apply()
        {
            System.IO.Directory.CreateDirectory(Directory);
            File.WriteAllText(DirectoryAndFileName, Content.Trim());
        }
    }
}