using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetRoleId(Guid id)
        {
            Id = id;
        }
    }
}