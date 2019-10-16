namespace Core
{
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Formatting.Elasticsearch;
    using Serilog.Sinks.Elasticsearch;
    using static System.Net.Security.SslPolicyErrors;
    using static Serilog.Events.LogEventLevel;
    using static Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseElasticsearchSerilog(
            this IHostBuilder hostBuilder,
            string appName,
            bool? serverCertificateValidationOverride = null)
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(
                    options: new ElasticsearchSinkOptions(context.Configuration.GetLogNodes())
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = ESv7,
                        IndexFormat = $"{appName.ToLowerInvariant().Replace('.', '-')}-logs",
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        ModifyConnectionSettings = x => x.ServerCertificateValidationCallback(
                            (o, certificate, arg3, arg4) => serverCertificateValidationOverride ?? arg4 == None)
                    }));
            return hostBuilder;
        }
    }
}
