using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IQuery, IRequest<GetRoleByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}