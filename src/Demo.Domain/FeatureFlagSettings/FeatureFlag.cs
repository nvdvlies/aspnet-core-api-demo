using Demo.Domain.Shared.Entities;
using System;
using System.Collections.Generic;

namespace Demo.Domain.FeatureFlagSettings
{
    public partial class FeatureFlag : AuditableEntity
    {
        public FeatureFlag()
        {
            EnabledForUsers = new List<Guid>();
        }

        public string Name { get; set; }
        public bool EnabledForAll { get; set; }
        public List<Guid> EnabledForUsers { get; set; }
    }
}