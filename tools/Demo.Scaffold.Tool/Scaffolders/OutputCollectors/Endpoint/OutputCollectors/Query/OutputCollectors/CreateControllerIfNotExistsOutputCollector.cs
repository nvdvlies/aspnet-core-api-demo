using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.OutputCollectors
{
    internal class CreateControllerIfNotExistsOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);

            if (!context.DoesControllerAlreadyExist(controllerName))
            {
                changes.Add(new CreateNewClass(
                    context.GetControllersDirectory(),
                    context.GetControllerFileName(controllerName),
                    GetTemplate(controllerName)
                ));
            }

            return changes;
        }

        private static string GetTemplate(string controllerName)
        {
            var code = @"
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers
{
    public class %NAME%Controller : ApiControllerBase
    {
        %SCAFFOLDMARKER%
    }
}
";
            code = code.Replace("%NAME%", controllerName);
            code = code.Replace("%SCAFFOLDMARKER%", Tool.Constants.ScaffoldMarkerEndpoint);
            return code;
        }
    }
}