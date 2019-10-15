namespace Core
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.ApplicationInsights;
    using static Microsoft.Extensions.Logging.LogLevel;

    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddApplicationInsightsLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, Trace);
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}
