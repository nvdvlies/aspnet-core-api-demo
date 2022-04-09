using Demo.Application.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings.Dtos
{
    internal class FeatureFlagDto : AuditableEntityDto
    {
        public string Name { get; set; }
        public bool EnabledForAll { get; set; }
        public List<Guid> EnabledForUsers { get; set; }
    }
}
