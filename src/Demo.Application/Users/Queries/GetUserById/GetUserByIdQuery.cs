using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IQuery, IRequest<GetUserByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}