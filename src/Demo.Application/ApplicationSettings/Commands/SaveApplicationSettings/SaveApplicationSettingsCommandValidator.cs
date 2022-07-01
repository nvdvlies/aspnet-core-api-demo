using FluentValidation;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommandValidator : AbstractValidator<SaveApplicationSettingsCommand>
    {
        public SaveApplicationSettingsCommandValidator()
        {
            RuleFor(x => x.Settings).NotNull();
        }
    }
}