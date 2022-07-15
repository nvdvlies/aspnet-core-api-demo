using System.Collections.Generic;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.CurrentUser.Queries.GetCurrentUserPermissions
{
    public class GetCurrentUserPermissionsQueryHandler : IRequestHandler<GetCurrentUserPermissionsQuery, GetCurrentUserPermissionsQueryResult>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IUserPermissionsProvider _userPermissionsProvider;
        private readonly IMapper _mapper;

        public GetCurrentUserPermissionsQueryHandler(
            ICurrentUserIdProvider currentUserIdProvider,
            IUserPermissionsProvider userPermissionsProvider,
            IMapper mapper
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _userPermissionsProvider = userPermissionsProvider;
            _mapper = mapper;
        }

        public async Task<GetCurrentUserPermissionsQueryResult> Handle(GetCurrentUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var userPermissions = await _userPermissionsProvider.GetAsync(_currentUserIdProvider.Id, cancellationToken);

            return new GetCurrentUserPermissionsQueryResult
            {
                Permissions = _mapper.Map<IEnumerable<PermissionDto>>(userPermissions)
            };
        }
    }
}