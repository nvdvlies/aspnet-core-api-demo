using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.ApplicationSettings.Queries.GetApplicationSettings;

public class GetApplicationSettingsQuery : IQuery, IRequest<GetApplicationSettingsQueryResult>
{
}
