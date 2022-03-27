using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : ICommand, IRequest<CreateRoleResponse>
    {
        public string Name { get; set; }
    }
}