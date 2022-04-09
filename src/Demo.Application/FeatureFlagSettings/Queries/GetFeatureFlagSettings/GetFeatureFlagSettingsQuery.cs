using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.FeatureFlagSettings.Queries.GetFeatureFlagSettings
{
    public class GetFeatureFlagSettingsQuery : IQuery, IRequest<GetFeatureFlagSettingsQueryResult>
    {
    }
}