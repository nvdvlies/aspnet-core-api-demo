using System.Collections.Generic;
using Demo.Application.Roles.Commands.CreateRole.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : ICommand, IRequest<CreateRoleResponse>
    {
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public List<CreateRoleCommandRolePermission> RolePermissions { get; set; }
    }
}
