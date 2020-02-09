namespace Microsoft.Extensions.Logging
{
    using ApplicationInsights;
    using JetBrains.Annotations;
    using static LogLevel;

    /// <summary>A class with methods that extend <see cref="ILoggingBuilder"/>.</summary>
    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
        /// <summary>Adds Application Insights logging to the <paramref name="loggingBuilder"/> using and <see cref="ApplicationInsightsLoggerProvider"/>.</summary>
        /// <param name="loggingBuilder">The logging builder.</param>
        /// <param name="category">The category.</param>
        /// <param name="level">The level.</param>
        /// <returns>The <paramref name="loggingBuilder"/>.</returns>
        public static ILoggingBuilder AddApplicationInsightsLogging(
            this ILoggingBuilder loggingBuilder,
            string category = "",
            LogLevel level = Trace)
        {
            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(category, level);
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}
