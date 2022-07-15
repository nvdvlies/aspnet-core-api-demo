using System;
using System.IO;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.InputCollectors
{
    internal class SolutionDirectoryInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            while (!HasValidPathToSolutionRootDirectory(context))
            {
                var path = AnsiConsole.Ask<string>($"In which directory is '{Constants.SolutionName}' located?");
                context.AppSettings.PathToSolutionRootDirectory = path;

                if (!HasValidPathToSolutionRootDirectory(context))
                {
                    Console.WriteLine($"{Constants.SolutionName} not found in directory '{path}'.");
                }
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
