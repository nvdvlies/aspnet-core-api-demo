using System.IO;

namespace Demo.Scaffold.Tool.Changes
{
    internal class BaseChange
    {
        protected string Directory { get; }
        protected string FileName { get; }
        protected string Content { get; }

        public BaseChange(string directory, string fileName, string content)
        {
            Directory = directory;
            FileName = fileName;
            Content = content;
        }

        public string DirectoryAndFileName => Path.Combine(Directory, FileName);
    }
}
