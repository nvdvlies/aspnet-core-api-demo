using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetUserId(Guid id)
        {
            Id = id;
        }
    }
}
