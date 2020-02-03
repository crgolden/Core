namespace Microsoft.Extensions.Logging
{
    using ApplicationInsights;
    using JetBrains.Annotations;
    using static System.String;
    using static LogLevel;

    /// <summary>A class with methods that extend <see cref="ILoggingBuilder"/>.</summary>
    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
        /// <summary>Adds Application Insights logging to the <paramref name="loggingBuilder"/> using and <see cref="ApplicationInsightsLoggerProvider"/>.</summary>
        /// <param name="loggingBuilder">The logging builder.</param>
        /// <returns>The <paramref name="loggingBuilder"/>.</returns>
        public static ILoggingBuilder AddApplicationInsightsLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(Empty, Trace);
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}
