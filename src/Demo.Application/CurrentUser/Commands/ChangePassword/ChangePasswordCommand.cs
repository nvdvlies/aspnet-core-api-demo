using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.CurrentUser.Commands.ChangePassword
{
    public class ChangePasswordCommand : ICommand, IRequest<Unit>
    {
    }
}