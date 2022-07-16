using System;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Commands.SaveFeatureFlagSettings.Dtos;

public class SaveFeatureFlagSettingsCommandSettingsFeatureFlagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool EnabledForAll { get; set; }
    public List<Guid> EnabledForUsers { get; set; }
}
