using System;
using System.IO;
using System.Linq;

namespace Demo.Scaffold.Tool.Changes
{
    internal class UpdateExistingClassAtMarker : BaseChange, IChange
    {
        private readonly string _marker;

        public UpdateExistingClassAtMarker(string directory, string fileName, string marker, string content)
            : base(directory, fileName, content)
        {
            _marker = marker;
        }

        public string Description => $"Modify: {RelativePathFromSolutionDirectory}";

        public void Apply()
        {
            var content = File.ReadAllText(DirectoryAndFileName);
            var markerWithPrecedingTabs = File.ReadAllLines(DirectoryAndFileName).Single(lines => lines.Contains(_marker));
            content = content.Replace(markerWithPrecedingTabs, $"{Content}{Environment.NewLine}{markerWithPrecedingTabs}");
            File.WriteAllText(DirectoryAndFileName, content);
        }
    }
}
