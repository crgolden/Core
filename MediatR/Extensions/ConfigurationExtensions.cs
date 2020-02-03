namespace Microsoft.Extensions.Configuration
{
    using System;
    using Core;
    using JetBrains.Annotations;

    /// <summary>A class with methods that extend <see cref="IConfiguration"/>.</summary>
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        /// <summary>Gets the <see cref="MediatROptions"/> configuration section.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The <paramref name="configuration"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The <see cref="MediatROptions"/> configuration section doesn't exist.</exception>
        public static IConfigurationSection GetMediatROptionsSection(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(MediatROptions));
            if (!section.Exists())
            {
                throw new ArgumentException($"{nameof(MediatROptions)} section doesn't exist", nameof(configuration));
            }

            return section;
        }
    }
}
