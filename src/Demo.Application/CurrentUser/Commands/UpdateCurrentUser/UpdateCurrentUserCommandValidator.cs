using FluentValidation;

namespace Demo.Application.CurrentUser.Commands.UpdateCurrentUser;

public class UpdateCurrentUserCommandValidator : AbstractValidator<UpdateCurrentUserCommand>
{
    public UpdateCurrentUserCommandValidator()
    {
        RuleFor(x => x.FamilyName).NotEmpty();
    }
}
