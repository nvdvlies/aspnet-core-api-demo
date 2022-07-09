using System;
using System.Collections.Generic;
using Demo.Application.Shared.Interfaces;
using Demo.Application.Users.Commands.CreateUser.Dtos;
using Demo.Application.Users.Queries.GetUserById.Dtos;
using Demo.Domain.User;
using MediatR;

namespace Demo.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand, IRequest<CreateUserResponse>
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
        public GenderEnum? Gender { get; set; }
        public UserTypeEnum UserType { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<CreateUserCommandUserRole> UserRoles { get; set; }
    }
}