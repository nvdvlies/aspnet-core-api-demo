using System;
using System.Collections.Generic;

namespace Demo.Domain.Role.Seed
{
    public static class PermissionsSeed
    {
        public static readonly Permission CustomersRead = new()
        {
            Id = Guid.Parse("bdd6b139-be77-4302-b80e-c1bce405ada5"),
            Name = Permissions.CustomersRead,
            PermissionGroupId = PermissionGroupsSeed.Customers.Id
        };

        public static readonly Permission CustomersWrite = new()
        {
            Id = Guid.Parse("c97b9d9d-6611-4a26-a1b4-43708402a49a"),
            Name = Permissions.CustomersWrite,
            PermissionGroupId = PermissionGroupsSeed.Customers.Id
        };

        public static readonly Permission InvoicesRead = new()
        {
            Id = Guid.Parse("931d572a-f85b-4dbb-a32d-8fee11e0e28d"),
            Name = Permissions.InvoicesRead,
            PermissionGroupId = PermissionGroupsSeed.Invoices.Id
        };

        public static readonly Permission InvoicesWrite = new()
        {
            Id = Guid.Parse("d5d6786c-ca5d-476e-b7a9-ccf67422b98d"),
            Name = Permissions.InvoicesWrite,
            PermissionGroupId = PermissionGroupsSeed.Invoices.Id
        };

        public static readonly Permission FeatureFlagSettingsRead = new()
        {
            Id = Guid.Parse("537d9994-517f-490b-8c7e-5da886e80d44"),
            Name = Permissions.FeatureFlagSettingsRead,
            PermissionGroupId = PermissionGroupsSeed.FeatureFlagSettings.Id
        };

        public static readonly Permission FeatureFlagSettingsWrite = new()
        {
            Id = Guid.Parse("7f77e408-04ce-496c-b347-bac63b0bc870"),
            Name = Permissions.FeatureFlagSettingsWrite,
            PermissionGroupId = PermissionGroupsSeed.FeatureFlagSettings.Id
        };

        public static readonly Permission UsersRead = new()
        {
            Id = Guid.Parse("286864f7-1c3c-4ca5-9ae0-5efe8b56bf5e"),
            Name = Permissions.UsersRead,
            PermissionGroupId = PermissionGroupsSeed.Users.Id
        };

        public static readonly Permission UsersWrite = new()
        {
            Id = Guid.Parse("b274daec-0a76-4bcc-b268-09768517d265"),
            Name = Permissions.UsersWrite,
            PermissionGroupId = PermissionGroupsSeed.Users.Id
        };

        public static readonly Permission ApplicationSettingsRead = new()
        {
            Id = Guid.Parse("db7b21d3-41ee-44d2-8218-78ef78f262d3"),
            Name = Permissions.ApplicationSettingsRead,
            PermissionGroupId = PermissionGroupsSeed.ApplicationSettings.Id
        };

        public static readonly Permission ApplicationSettingsWrite = new()
        {
            Id = Guid.Parse("29ece69e-c315-4902-b959-82790a38dc8a"),
            Name = Permissions.ApplicationSettingsWrite,
            PermissionGroupId = PermissionGroupsSeed.ApplicationSettings.Id
        };

        public static readonly Permission RolesRead = new()
        {
            Id = Guid.Parse("7baf877f-dda2-4940-b7e5-38274fe7f28b"),
            Name = Permissions.RolesRead,
            PermissionGroupId = PermissionGroupsSeed.Roles.Id
        };

        public static readonly Permission RolesWrite = new()
        {
            Id = Guid.Parse("692a27f6-c217-4bfe-a210-8ab19d809199"),
            Name = Permissions.RolesWrite,
            PermissionGroupId = PermissionGroupsSeed.Roles.Id
        };

        public static List<Permission> All => new()
        {
            CustomersRead,
            CustomersWrite,
            InvoicesRead,
            InvoicesWrite,
            FeatureFlagSettingsRead,
            FeatureFlagSettingsWrite,
            UsersRead,
            UsersWrite,
            ApplicationSettingsRead,
            ApplicationSettingsWrite,
            RolesRead,
            RolesWrite
        };
    }
}
