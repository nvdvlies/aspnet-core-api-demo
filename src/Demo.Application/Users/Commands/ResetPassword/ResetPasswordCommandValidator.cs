using FluentValidation;

namespace Demo.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            // RuleFor(x => x.Id).NotEmpty();
        }
    }
}