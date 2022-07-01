using FluentValidation;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings
{
    public class SaveFeatureFlagSettingsCommandValidator : AbstractValidator<SaveFeatureFlagSettingsCommand>
    {
        public SaveFeatureFlagSettingsCommandValidator()
        {
            RuleFor(x => x.Settings).NotNull();
        }
    }
}