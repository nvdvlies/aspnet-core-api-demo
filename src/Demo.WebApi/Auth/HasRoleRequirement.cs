using System;
using Microsoft.AspNetCore.Authorization;

namespace Demo.WebApi.Auth
{
    public class HasRoleRequirement : IAuthorizationRequirement
    {
        public HasRoleRequirement(string role, string issuer)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }

        public string Issuer { get; }
        public string Role { get; }
    }
}