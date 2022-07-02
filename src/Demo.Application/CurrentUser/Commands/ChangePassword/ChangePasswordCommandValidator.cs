using FluentValidation;

namespace Demo.Application.CurrentUser.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            // RuleFor(x => x.Id).NotEmpty();
        }
    }
}