using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos
{
    public class FeatureFlagDto : AuditableEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool EnabledForAll { get; set; }
        public List<Guid> EnabledForUsers { get; set; }
    }
}
