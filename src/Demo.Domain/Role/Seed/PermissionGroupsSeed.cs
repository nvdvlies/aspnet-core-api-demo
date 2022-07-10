using System;
using System.Collections.Generic;

namespace Demo.Domain.Role.Seed
{
    public static class PermissionGroupsSeed
    {
        public static readonly PermissionGroup Customers = new()
            { Id = Guid.Parse("bce43d29-e527-4364-890a-0a49224abf74"), Name = "Customers" };

        public static readonly PermissionGroup Invoices = new()
            { Id = Guid.Parse("4b4e2d70-02dc-43ac-a8bc-c75c25e1e71d"), Name = "Invoices" };

        public static readonly PermissionGroup FeatureFlags = new()
            { Id = Guid.Parse("6fd39917-5f96-472d-ac69-d2a8c56880b7"), Name = "FeatureFlags" };

        public static readonly PermissionGroup Users = new()
            { Id = Guid.Parse("9b621e5b-e277-4c88-88d2-18a7befb45aa"), Name = "Users" };

        public static readonly PermissionGroup ApplicationSettings = new()
            { Id = Guid.Parse("d3fda6f7-4a23-4a2c-bfcf-abd0aec25774"), Name = "ApplicationSettings" };

        public static readonly PermissionGroup Roles = new()
            { Id = Guid.Parse("7af7d630-e85d-4183-966f-a2cf4a3d67f0"), Name = "Roles" };

        public static List<PermissionGroup> All => new()
        {
            Customers, Invoices, FeatureFlags, Users, ApplicationSettings, Roles
        };
    }
}