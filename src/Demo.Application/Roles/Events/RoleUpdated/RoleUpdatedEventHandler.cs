using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.Role;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Roles.Events.RoleUpdated;

public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<RoleUpdatedEventHandler> _logger;
    private readonly IPermissionsProvider _permissionsProvider;
    private readonly IRolesProvider _rolesProvider;

    public RoleUpdatedEventHandler(
        ILogger<RoleUpdatedEventHandler> logger,
        IEventHubContext eventHubContext,
        IRolesProvider rolesProvider,
        IPermissionsProvider permissionsProvider
    )
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
        _rolesProvider = rolesProvider;
        _permissionsProvider = permissionsProvider;
    }

    public async Task Handle(RoleUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(RoleUpdatedEvent)}");
        await _rolesProvider.GetAsync(true, cancellationToken);
        await _permissionsProvider.GetAsync(true, cancellationToken);
        await _eventHubContext.All.RoleUpdated(@event.Data.Id, @event.Data.UpdatedBy);
    }
}
