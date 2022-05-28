using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }

        public void SetRoleId(Guid id)
        {
            Id = id;
        }
    }
}