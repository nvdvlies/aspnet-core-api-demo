using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Permissions.Queries.GetAllPermissionGroups;

public class
    GetAllPermissionGroupsQueryHandler : IRequestHandler<GetAllPermissionGroupsQuery,
        GetAllPermissionGroupsQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IPermissionGroupsProvider _permissionGroupsProvider;

    public GetAllPermissionGroupsQueryHandler(
        IPermissionGroupsProvider permissionGroupsProvider,
        IMapper mapper
    )
    {
        _permissionGroupsProvider = permissionGroupsProvider;
        _mapper = mapper;
    }

    public async Task<GetAllPermissionGroupsQueryResult> Handle(GetAllPermissionGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var permissionGroups = await _permissionGroupsProvider.GetAsync(cancellationToken);

        return new GetAllPermissionGroupsQueryResult
        {
            PermissionGroups = _mapper.Map<IEnumerable<PermissionGroupDto>>(permissionGroups)
        };
    }
}
