using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Models;
using Demo.Scaffold.Tool.Scaffolders.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders
{
    internal class ScaffolderService
    {
        private readonly AppSettings _appSettings;

        private readonly List<IInputCollector> _inputCollectors = new()
        {
            new SolutionDirectoryInputCollector(), new ScaffoldTypeInputCollector()
        };

        private readonly List<IOutputCollector> _outputCollectors = new() { new ScaffolderTypeOutputCollector() };

        public ScaffolderService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private ScaffolderContext Context { get; set; }

        public void Run()
        {
            Console.Clear();

            Context = new ScaffolderContext(_appSettings);

            foreach (var inputCollector in _inputCollectors)
            {
                inputCollector.CollectInput(Context);
            }

            var changes = _outputCollectors
                .Select(x => x.CollectChanges(Context))
                .SelectMany(x => x)
                .Where(x => x != null)
                .ToList();

            if (ConfirmChanges(changes))
            {
                ApplyChanges(changes);
                Console.WriteLine("Done.");
            }

            if (AnsiConsole.Confirm("Do you want to scaffold something else?", false))
            {
                Run();
            }

            SaveUserAppSettings();
        }

        private bool ConfirmChanges(IEnumerable<IChange> changes)
        {
            Console.WriteLine();
            Console.WriteLine("Pending changes:");
            Console.WriteLine();
            foreach (var change in changes
                         .GroupBy(x => x.DirectoryAndFileName)
                         .Select(x => x.First())
                         .OrderBy(x => x.DirectoryAndFileName))
            {
                Console.WriteLine(
                    $" - {change.ModificationType}: {Path.GetRelativePath(_appSettings.PathToSolutionRootDirectory, change.DirectoryAndFileName)}");
            }

            Console.WriteLine();
            return AnsiConsole.Confirm("Apply pending changes?");
        }

        private void ApplyChanges(IEnumerable<IChange> changes)
        {
            foreach (var change in changes)
            {
                change.Apply();
            }
        }

        private void SaveUserAppSettings()
        {
            var fullPathAndFileName = Path.Combine(Directory.GetCurrentDirectory(), Constants.UserSettingsFileName);
            var serializedAppSettings =
                JsonSerializer.Serialize(Context.AppSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fullPathAndFileName, serializedAppSettings);
        }
    }
}
