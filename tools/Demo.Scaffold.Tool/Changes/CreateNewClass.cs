using System.IO;

namespace Demo.Scaffold.Tool.Changes
{
    internal class CreateNewClass : BaseChange, IChange
    {
        public CreateNewClass(string directory, string fileName, string content)
            : base(directory, fileName, content)
        {
        }

        public string Description => $"Create: {DirectoryAndFileName}";

        public void Apply()
        {
            System.IO.Directory.CreateDirectory(Directory);
            File.WriteAllText(DirectoryAndFileName, Content.Trim());
        }
    }
}
