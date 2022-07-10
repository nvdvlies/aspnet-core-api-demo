using System;
using System.Text.Json.Serialization;

namespace Demo.Domain.Role
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        [JsonInclude]
        public Role Role { get; internal set; }

        public Guid PermissionId { get; set; }
        [JsonInclude]
        public Permission Permission { get; internal set; }
    }
}