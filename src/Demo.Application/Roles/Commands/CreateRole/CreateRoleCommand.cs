using MediatR;
using System;

namespace Demo.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<CreateRoleResponse>
    {
        public string Name { get; set; }
    }
}