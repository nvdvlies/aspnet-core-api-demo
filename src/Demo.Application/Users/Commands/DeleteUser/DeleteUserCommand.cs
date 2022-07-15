using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetUserId(Guid id)
        {
            Id = id;
        }
    }
}
