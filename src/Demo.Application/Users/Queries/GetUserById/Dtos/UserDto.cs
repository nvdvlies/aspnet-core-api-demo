using System;
using System.Collections.Generic;
using Demo.Application.Shared.Dtos;

namespace Demo.Application.Users.Queries.GetUserById.Dtos
{
    public class UserDto : SoftDeleteEntityDto
    {
        public string ExternalId { get; set; }
        public string Fullname { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}