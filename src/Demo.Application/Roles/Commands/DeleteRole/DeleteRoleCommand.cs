using MediatR;
using System;

namespace Demo.Application.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetRoleId(Guid id)
        {
            Id = id;
        }
    }
}