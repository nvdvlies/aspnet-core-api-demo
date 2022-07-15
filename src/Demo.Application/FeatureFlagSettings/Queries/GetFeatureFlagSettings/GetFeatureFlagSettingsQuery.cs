using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsQuery : IQuery, IRequest<GetFeatureFlagSettingsQueryResult>
    {
    }
}
