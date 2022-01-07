using Demo.Scaffold.Tool.Scaffolders;
using System;
using System.IO;

namespace Demo.Scaffold.Tool.Helpers
{
    internal static class ScaffolderContextExtensions
    {
        public static string GetControllersDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.AppSettings.PathToSolutionRootDirectory, Constants.WebApiPath, "Controllers");
        }

        public static string GetControllerFileName(this ScaffolderContext context, string name)
        {
            return $"{name.Replace("Controller", string.Empty, StringComparison.CurrentCultureIgnoreCase)}Controller.cs";
        }

        public static string GetControllerFullPath(this ScaffolderContext context, string name)
        {
            return Path.Combine(context.GetControllersDirectory(), context.GetControllerFileName(name));
        }

        public static bool DoesControllerAlreadyExist(this ScaffolderContext context, string name)
        {
            return File.Exists(context.GetControllerFullPath(name));
        }

        public static string GetDomainDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.AppSettings.PathToSolutionRootDirectory, Constants.DomainPath);
        }

        public static string GetEntityDirectory(this ScaffolderContext context, string enityName)
        {
            return Path.Combine(context.GetDomainDirectory(), enityName);
        }

        public static string GetInfrastructureDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.AppSettings.PathToSolutionRootDirectory, Constants.InfrastructurePath);
        }

        public static string GetAuditloggerDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.GetInfrastructureDirectory(), "Auditlogging");
        }

        public static string GetPersistenceDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.GetInfrastructureDirectory(), "Persistence");
        }

        public static string GetEntityTypeConfigurationDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.GetPersistenceDirectory(), "Configuration");
        }

        public static string GetApplicationDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.AppSettings.PathToSolutionRootDirectory, Constants.ApplicationPath);
        }

        public static string GetCommandDirectory(this ScaffolderContext context, string constrollerName, string commandName)
        {
            return Path.Combine(context.GetApplicationDirectory(), constrollerName, "Commands", commandName);
        }

        public static string GetQueryDirectory(this ScaffolderContext context, string constrollerName, string queryName)
        {
            return Path.Combine(context.GetApplicationDirectory(), constrollerName, "Queries", queryName);
        }

        public static string GetEventsDirectory(this ScaffolderContext context)
        {
            return Path.Combine(context.AppSettings.PathToSolutionRootDirectory, Constants.EventsPath);
        }
    }
}
