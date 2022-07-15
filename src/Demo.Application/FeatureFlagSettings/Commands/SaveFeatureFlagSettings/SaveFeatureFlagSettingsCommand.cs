using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings
{
    public class SaveFeatureFlagSettingsCommand : ICommand, IRequest<Unit>
    {
        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }
        public SaveFeatureFlagSettingsCommandSettingsDto Settings { get; set; }
    }
}
