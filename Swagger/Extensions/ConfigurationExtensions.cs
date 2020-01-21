namespace Core.Extensions
{
    using System;
    using Core;
    using Microsoft.Extensions.Configuration;

    /// <summary>A class with methods that extend <see cref="IConfiguration"/>.</summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Gets the <see cref="SwaggerOptions"/> configuration section.</summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        /// <returns>The <see cref="SwaggerOptions"/> configuration section.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The <see cref="SwaggerOptions"/> configuration section doesn't exist.</exception>
        public static IConfigurationSection GetSwaggerOptionsSection(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(SwaggerOptions));
            if (!section.Exists())
            {
                throw new ArgumentException($"{nameof(SwaggerOptions)} section doesn't exist", nameof(configuration));
            }

            return section;
        }
    }
}
