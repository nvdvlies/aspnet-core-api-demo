using System;
using System.Collections.Generic;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.User
{
    public class User : SoftDeleteEntity, IQueryableEntity
    {
        public string ExternalId { get; set; }
        public string Fullname { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public UserType UserType { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}