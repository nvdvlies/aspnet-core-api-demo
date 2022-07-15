using FluentValidation;

namespace Demo.Application.UserPreferences.Commands.SaveUserPreferences
{
    public class SaveUserPreferencesCommandValidator : AbstractValidator<SaveUserPreferencesCommand>
    {
        public SaveUserPreferencesCommandValidator()
        {
            RuleFor(x => x.Preferences).NotNull();
        }
    }
}
