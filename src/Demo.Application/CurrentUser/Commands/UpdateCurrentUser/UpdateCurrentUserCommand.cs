using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Commands.UpdateCurrentUser;

public class UpdateCurrentUserCommand : ICommand, IRequest<Unit>
{
    public string GivenName { get; set; }
    public string MiddleName { get; set; }
    public string FamilyName { get; set; }
}
