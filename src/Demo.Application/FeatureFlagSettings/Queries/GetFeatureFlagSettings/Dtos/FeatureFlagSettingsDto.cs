using Demo.Application.Shared.Dtos;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos
{
    internal class FeatureFlagSettingsDto : AuditableEntityDto
    {
        public List<FeatureFlagDto> FeatureFlags { get; set; }
    }
}
