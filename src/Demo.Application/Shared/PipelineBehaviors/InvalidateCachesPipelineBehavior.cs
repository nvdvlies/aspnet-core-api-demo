using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.ApplicationSettings.Interfaces;
using Demo.Domain.FeatureFlagSettings.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.UserPreferences.Interfaces;
using Demo.Events.ApplicationSettings;
using Demo.Events.FeatureFlagSettings;
using Demo.Events.UserPreferences;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class InvalidateCachesPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Lazy<IApplicationSettingsProvider> _applicationSettingsProvider;
        private readonly Lazy<ICurrentUserIdProvider> _currentUserIdProvider;
        private readonly Lazy<IFeatureFlagSettingsProvider> _featureFlagSettingsProvider;
        private readonly ILogger<InvalidateCachesPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly Lazy<IOutboxEventCreatedEvents> _outboxEventCreatedEvents;
        private readonly Lazy<IUserPreferencesProvider> _userPreferencesProvider;

        public InvalidateCachesPipelineBehavior(
            ILogger<InvalidateCachesPipelineBehavior<TRequest, TResponse>> logger,
            Lazy<IOutboxEventCreatedEvents> outboxEventCreatedEvents,
            Lazy<IApplicationSettingsProvider> applicationSettingsProvider,
            Lazy<IUserPreferencesProvider> userPreferencesProvider,
            Lazy<IFeatureFlagSettingsProvider> featureFlagSettingsProvider,
            Lazy<ICurrentUserIdProvider> currentUserIdProvider
        )
        {
            _logger = logger;
            _outboxEventCreatedEvents = outboxEventCreatedEvents;
            _applicationSettingsProvider = applicationSettingsProvider;
            _userPreferencesProvider = userPreferencesProvider;
            _featureFlagSettingsProvider = featureFlagSettingsProvider;
            _currentUserIdProvider = currentUserIdProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (_outboxEventCreatedEvents.Value.Any(x =>
                    x.Data.EventType == typeof(ApplicationSettingsUpdatedEvent).FullName))
            {
                _logger.LogInformation("Refreshing application settings cache");
                await _applicationSettingsProvider.Value.GetAsync(true, cancellationToken);
            }

            if (_outboxEventCreatedEvents.Value.Any(x =>
                    x.Data.EventType == typeof(UserPreferencesUpdatedEvent).FullName))
            {
                _logger.LogInformation("Refreshing user preferences cache for user id '{userId}'",
                    _currentUserIdProvider.Value.Id);
                await _userPreferencesProvider.Value.GetAsync(true, cancellationToken);
            }

            if (_outboxEventCreatedEvents.Value.Any(x =>
                    x.Data.EventType == typeof(FeatureFlagSettingsUpdatedEvent).FullName))
            {
                _logger.LogInformation("Refreshing feature flag settings cache");
                await _featureFlagSettingsProvider.Value.GetAsync(true, cancellationToken);
            }

            return response;
        }
    }
}
