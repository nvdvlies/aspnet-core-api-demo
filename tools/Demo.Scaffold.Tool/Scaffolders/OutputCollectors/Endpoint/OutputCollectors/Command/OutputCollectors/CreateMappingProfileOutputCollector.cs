using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.OutputCollectors
{
    internal class CreateMappingProfileOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var commandEndpointType = context.Variables.Get<CommandEndpointTypes>(Constants.CommandEndpointType);

            if (commandEndpointType != CommandEndpointTypes.Create
                && commandEndpointType != CommandEndpointTypes.Update)
            {
                return changes;
            }

            var controllerName = context.Variables.Get<string>(Constants.ControllerName);
            var commandName = context.Variables.Get<string>(Constants.CommandName);

            changes.Add(new CreateNewClass(
                context.GetCommandDirectory(controllerName, commandName),
                $"{commandName}MappingProfile.cs",
                GetTemplate(controllerName, commandName, commandEndpointType)
            ));

            return changes;
        }

        private static string GetTemplate(string controllerName, string commandName,
            CommandEndpointTypes commandEndpointType)
        {
            var code = @"
using AutoMapper;
using Demo.Domain.%ENTITYNAME_GUESS%;

namespace Demo.Application.%CONTROLLERNAME%.Commands.%COMMANDNAME%
{
    public class %COMMANDNAME%MappingProfile : Profile
    {
        public %COMMANDNAME%MappingProfile()
        {
            CreateMap<%COMMANDNAME%Command, %ENTITYNAME_GUESS%>()
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedBy, opt => opt.Ignore())
                .ForMember(x => x.DeletedOn, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedOn, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.xmin, opt => opt.Ignore());
        }
    }
}
";

            code = code.Replace("%CONTROLLERNAME%", controllerName);
            code = code.Replace("%COMMANDNAME%", commandName);
            code = code.Replace("%ENTITYNAME_GUESS%",
                commandName.Replace("Create", string.Empty).Replace("Update", string.Empty));
            return code;
        }
    }
}
