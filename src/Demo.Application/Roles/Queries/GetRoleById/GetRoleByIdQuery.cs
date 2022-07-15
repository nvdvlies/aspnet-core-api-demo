using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IQuery, IRequest<GetRoleByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
}
