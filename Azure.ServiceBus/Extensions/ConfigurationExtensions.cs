namespace Microsoft.Extensions.Configuration
{
    using System;
    using Core;
    using global::Azure.Messaging.ServiceBus;
    using JetBrains.Annotations;

    /// <summary>A class with methods that extend <see cref="IConfiguration"/>.</summary>
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        /// <summary>Gets the <see cref="ServiceBusConnectionStringProperties"/> configuration section.</summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        /// <returns>The <see cref="ServiceBusConnectionStringProperties"/> configuration section.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The <see cref="ServiceBusConnectionStringProperties"/> configuration section doesn't exist.</exception>
        public static IConfigurationSection GetServiceBusConnectionStringPropertiesSection(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(ServiceBusConnectionStringProperties));
            if (!section.Exists())
            {
                throw new ArgumentException($"'{nameof(ServiceBusConnectionStringProperties)}' section doesn't exist", nameof(configuration));
            }

            return section;
        }
    }
}
