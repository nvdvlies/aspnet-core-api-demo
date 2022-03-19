using MediatR;
using System;

namespace Demo.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}