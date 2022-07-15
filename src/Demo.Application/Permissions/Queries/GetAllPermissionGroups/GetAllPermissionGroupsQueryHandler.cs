using System.Collections.Generic;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Permissions.Queries.GetAllPermissionGroups
{
    public class GetAllPermissionGroupsQueryHandler : IRequestHandler<GetAllPermissionGroupsQuery, GetAllPermissionGroupsQueryResult>
    {
        private readonly IPermissionGroupsProvider _permissionGroupsProvider;
        private readonly IMapper _mapper;

        public GetAllPermissionGroupsQueryHandler(
            IPermissionGroupsProvider permissionGroupsProvider,
            IMapper mapper
        )
        {
            _permissionGroupsProvider = permissionGroupsProvider;
            _mapper = mapper;
        }

        public async Task<GetAllPermissionGroupsQueryResult> Handle(GetAllPermissionGroupsQuery request, CancellationToken cancellationToken)
        {
            var permissionGroups = await _permissionGroupsProvider.GetAsync(cancellationToken);

            return new GetAllPermissionGroupsQueryResult
            {
                PermissionGroups = _mapper.Map<IEnumerable<PermissionGroupDto>>(permissionGroups)
            };
        }
    }
}