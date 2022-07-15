using System;
using System.Collections.Generic;
using Demo.Domain.Shared.Entities;

namespace Demo.Domain.FeatureFlagSettings
{
    public class FeatureFlag : AuditableEntity
    {
        public FeatureFlag()
        {
            EnabledForUsers = new List<Guid>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool EnabledForAll { get; set; }
        public List<Guid> EnabledForUsers { get; set; }
    }
}
