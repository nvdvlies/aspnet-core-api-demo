using Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings
{
    public class SaveFeatureFlagSettingsCommand : ICommand, IRequest<Unit>
    {
        public byte[] Timestamp { get; set; }
        public List<SaveFeatureFlagSettingsCommandFeatureFlagDto> FeatureFlags { get; set; }
    }
}