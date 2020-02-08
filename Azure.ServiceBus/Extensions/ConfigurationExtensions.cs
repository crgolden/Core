namespace Microsoft.Extensions.Configuration
{
    using System;
    using Core;
    using JetBrains.Annotations;

    /// <summary>A class with methods that extend <see cref="IConfiguration"/>.</summary>
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        /// <summary>Gets the <see cref="ServiceBusOptions"/> configuration section.</summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        /// <returns>The <see cref="ServiceBusOptions"/> configuration section.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The <see cref="ServiceBusOptions"/> configuration section doesn't exist.</exception>
        public static IConfigurationSection GetServiceBusOptionsOptionsSection(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(ServiceBusOptions));
            if (!section.Exists())
            {
                throw new ArgumentException($"'{nameof(ServiceBusOptions)}' section doesn't exist", nameof(configuration));
            }

            return section;
        }
    }
}
