using System;
using System.Collections.Generic;
using Demo.Application.Roles.Commands.UpdateRole.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public List<UpdateRoleCommandRolePermission> RolePermissions { get; set; }

        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }

        public void SetRoleId(Guid id)
        {
            Id = id;
        }
    }
}
