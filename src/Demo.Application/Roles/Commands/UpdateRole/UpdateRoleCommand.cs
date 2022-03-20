using MediatR;
using System;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }
        public string Name { get; set; }

        public void SetRoleId(Guid id)
        {
            Id = id;
        }
    }
}