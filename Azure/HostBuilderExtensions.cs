namespace Core
{
    using System;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using static Serilog.Events.LogEventLevel;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseAzureSerilog(this IHostBuilder hostBuilder, string appName)
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"D:\\home\\LogFiles\\Application\\{appName}.txt",
                    fileSizeLimitBytes: 1_000_000,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    rollOnFileSizeLimit: true));
            return hostBuilder;
        }
    }
}
