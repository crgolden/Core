namespace Core
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.SnapshotCollector;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class SnapshotCollectorTelemetryProcessorFactory : ITelemetryProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SnapshotCollectorTelemetryProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITelemetryProcessor Create(ITelemetryProcessor nextProcessor)
        {
            var options = _serviceProvider.GetService<IOptions<SnapshotCollectorConfiguration>>();
            return options != null
                ? new SnapshotCollectorTelemetryProcessor(nextProcessor, options.Value)
                : new SnapshotCollectorTelemetryProcessor(nextProcessor);
        }
    }
}
