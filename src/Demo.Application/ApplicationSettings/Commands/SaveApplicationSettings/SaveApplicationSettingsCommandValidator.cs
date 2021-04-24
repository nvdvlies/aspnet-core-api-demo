using FluentValidation;

namespace Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings
{
    public class SaveApplicationSettingsCommandValidator : AbstractValidator<SaveApplicationSettingsCommand>
    {
        public SaveApplicationSettingsCommandValidator()
        {
        }
    }
}