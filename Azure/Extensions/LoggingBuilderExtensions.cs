﻿namespace Microsoft.Extensions.Logging
{
    using System;
    using JetBrains.Annotations;

    /// <summary>A class with methods that extend <see cref="ILoggingBuilder"/>.</summary>
    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
        /// <summary>Adds the azure logging.</summary>
        /// <param name="loggingBuilder">The logging builder.</param>
        /// <returns>The <paramref name="loggingBuilder"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="loggingBuilder"/> is <see langword="null" />.</exception>
        public static ILoggingBuilder AddAzureLogging(this ILoggingBuilder loggingBuilder)
        {
            if (loggingBuilder == default)
            {
                throw new ArgumentNullException(nameof(loggingBuilder));
            }

            loggingBuilder.AddAzureWebAppDiagnostics();
            return loggingBuilder;
        }
    }
}
