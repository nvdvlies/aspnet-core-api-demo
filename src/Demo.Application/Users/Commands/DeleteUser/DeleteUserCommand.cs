using MediatR;
using System;

namespace Demo.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetUserId(Guid id)
        {
            Id = id;
        }
    }
}