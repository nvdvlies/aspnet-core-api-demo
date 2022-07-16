using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Users.Queries.GetUserById;

public class GetUserByIdQuery : IQuery, IRequest<GetUserByIdQueryResult>
{
    public Guid Id { get; set; }
}
