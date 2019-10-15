namespace Core
{
    using Microsoft.Extensions.Logging;

    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddAzureLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddAzureWebAppDiagnostics();
            return loggingBuilder;
        }
    }
}
