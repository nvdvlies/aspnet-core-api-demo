using System;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos
{
    public class SaveFeatureFlagSettingsCommandFeatureFlagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool EnabledForAll { get; set; }
        public List<Guid> EnabledForUsers { get; set; }
    }
}
