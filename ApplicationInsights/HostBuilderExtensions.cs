namespace Core
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using static Serilog.Events.LogEventLevel;
    using static Serilog.TelemetryConverter;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseApplicationInsightsSerilog(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(
                    telemetryConfiguration: TelemetryConfiguration.CreateDefault(),
                    telemetryConverter: Traces));
            return hostBuilder;
        }
    }
}
