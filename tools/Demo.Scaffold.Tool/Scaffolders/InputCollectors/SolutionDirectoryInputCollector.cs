using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.InputCollectors
{
    internal class SolutionDirectoryInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            while (!HasValidPathToSolutionRootDirectory(context))
            {
                var path = AnsiConsole.Ask<string>($"In which directory is '{Constants.SolutionName}' is located?");
                context.AppSettings.PathToSolutionRootDirectory = path;
            }
        }

        private bool HasValidPathToSolutionRootDirectory(ScaffolderContext context)
        {
            var path = context.AppSettings.PathToSolutionRootDirectory;
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            if (!Directory.Exists(path))
            {
                return false;
            }
            if (!File.Exists(Path.Combine(path, Constants.SolutionName)))
            {
                return false;
            }
            return true;
        }
    }
}
