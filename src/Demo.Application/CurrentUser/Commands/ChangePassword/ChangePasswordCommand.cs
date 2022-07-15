using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Commands.ChangePassword
{
    public class ChangePasswordCommand : ICommand, IRequest<Unit>
    {
    }
}
