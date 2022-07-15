using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Permissions.Queries.GetAllPermissionGroups
{
    public class GetAllPermissionGroupsQuery : IQuery, IRequest<GetAllPermissionGroupsQueryResult>
    {
    }
}