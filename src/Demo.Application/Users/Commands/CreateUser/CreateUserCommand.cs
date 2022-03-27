using Demo.Application.Shared.Interfaces;
using Demo.Application.Users.Commands.CreateUser.Dtos;
using Demo.Application.Users.Queries.GetUserById.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace Demo.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand, IRequest<CreateUserResponse>
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ZoneInfo { get; set; }
        public string Locale { get; set; }
        public List<CreateUserCommandUserRole> UserRoles { get; set; }
    }
}