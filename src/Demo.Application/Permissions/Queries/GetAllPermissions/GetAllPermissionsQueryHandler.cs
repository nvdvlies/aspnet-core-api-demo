using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Permissions.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, GetAllPermissionsQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IPermissionsProvider _permissionsProvider;

    public GetAllPermissionsQueryHandler(
        IPermissionsProvider permissionsProvider,
        IMapper mapper)
    {
        _permissionsProvider = permissionsProvider;
        _mapper = mapper;
    }

    public async Task<GetAllPermissionsQueryResult> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionsProvider.GetAsync(cancellationToken);

        return new GetAllPermissionsQueryResult { Permissions = _mapper.Map<IEnumerable<PermissionDto>>(permissions) };
    }
}
