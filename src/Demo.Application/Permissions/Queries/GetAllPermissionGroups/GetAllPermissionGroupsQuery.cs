using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Permissions.Queries.GetAllPermissionGroups;

public class GetAllPermissionGroupsQuery : IQuery, IRequest<GetAllPermissionGroupsQueryResult>
{
}
